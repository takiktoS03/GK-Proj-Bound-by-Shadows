using System.Collections;
using UnityEngine;

namespace EthanTheHero
{
    /**
     * @class PlayerMovement
     * @brief Odpowiada za ruch postaci gracza (bieganie, skok, dash, wall slide, wall jump).
     *
     * Bazuje na danych z `PlayerMovementData`. Obsługuje również dźwięki (kroki, dash, skok),
     * stan UI oraz interakcję z kolizjami gruntu i ścian.
     */
    public class PlayerMovement : MonoBehaviour
    {
        #region FIELD

        [SerializeField] private PlayerMovementData data;                  ///< Dane konfiguracyjne ruchu gracza.
        [SerializeField] private float lastOnGroundTime;                   ///< Czas od ostatniego kontaktu z ziemią.
        [SerializeField] private Transform groundCheckPoint;              ///< Punkt sprawdzania kolizji z ziemią.
        [SerializeField] private Vector2 groundCheckSize = new(0.49f, 0.03f); ///< Rozmiar obszaru sprawdzania ziemi.
        [SerializeField] private LayerMask groundLayer;                   ///< Warstwa oznaczająca ziemię.
        [SerializeField] private LayerMask wallLayer;                     ///< Warstwa oznaczająca ściany.
        [SerializeField] private Transform WallCheck;                     ///< Punkt sprawdzania kolizji ze ścianą.

        [HideInInspector] public Vector2 move;                            ///< Kierunek ruchu gracza.

        private Rigidbody2D myBody;
        private Animator myAnim;

        // Dash
        [HideInInspector] public bool isDashing;
        private bool canDash = true;
        private bool dashButtonPressed;

        // Jump
        [HideInInspector] public bool grounded;
        [HideInInspector] public bool isJumping;
        private bool jumpButtonPressed;

        // Wall Sliding and Wall Jump
        [HideInInspector] public bool wallSlidingEnabled = true;
        [HideInInspector] public bool wallJump;
        [HideInInspector] public bool wallSliding;
        private RaycastHit2D wall;
        private float jumpTime;

        private PlayerHealth healthComponent;
        private bool stepSoundPlaying = false;

        /**
         * @brief Zatrzymuje dźwięk kroków na krótki czas.
         */
        private IEnumerator ResetStepSound()
        {
            yield return new WaitForSeconds(0.3f);
            stepSoundPlaying = false;
        }

        #endregion

        #region MONOBEHAVIOUR

        void Awake()
        {
            myBody = GetComponent<Rigidbody2D>();
            myAnim = GetComponent<Animator>();
            healthComponent = GetComponent<PlayerHealth>();
        }

        void Update()
        {
            if (UIStateManager.isUIOpen || isDashing || wallJump ||
                myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01") ||
                myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02") ||
                myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
                return;

            lastOnGroundTime -= Time.deltaTime;

            // Input Handler
            move.x = Input.GetAxisRaw("Horizontal");
            dashButtonPressed = Input.GetKeyDown(KeyCode.W);
            jumpButtonPressed = Input.GetButtonDown("Jump");

            jump();

            if (move.x != 0)
                CheckDirectionToFace(move.x > 0);

            if (dashButtonPressed && canDash && !wallSliding && healthComponent.currentStamina >= data.dashCost)
            {
                healthComponent.TakeStamina(data.dashCost);
                StartCoroutine(dash());
            }

            if (wallSliding && jumpButtonPressed)
                StartCoroutine(wallJumpMechanic());
        }

        void FixedUpdate()
        {
            if (UIStateManager.isUIOpen || isDashing || wallJump ||
                myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01") ||
                myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02") ||
                myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
                return;

            if (!wallSliding)
                run(1);

            // Kroki
            if (move.x != 0 && grounded)
                SoundManager.Instance?.StartSteps();
            else
                SoundManager.Instance?.StopSteps();

            // Sprawdzanie kolizji z ziemią
            grounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);
            if (grounded)
                lastOnGroundTime = 0.1f;

            WallSlidngMechanic();
        }

        #endregion

        #region RUN

        /**
         * @brief Odpowiada za ruch poziomy postaci.
         * @param lerpAmount Poziom wygładzenia ruchu (interpolacja).
         */
        private void run(float lerpAmount)
        {
            float targetSpeed = move.x * data.runMaxSpeed;
            float accelRate;

            targetSpeed = Mathf.Lerp(myBody.linearVelocity.x, targetSpeed, lerpAmount);

            if (lastOnGroundTime > 0)
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount : data.runDeccelAmount;
            else
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount * data.accelInAir : data.runDeccelAmount * data.deccelInAir;

            if (data.doConserveMomentum &&
                Mathf.Abs(myBody.linearVelocity.x) > Mathf.Abs(targetSpeed) &&
                Mathf.Sign(myBody.linearVelocity.x) == Mathf.Sign(targetSpeed) &&
                Mathf.Abs(targetSpeed) > 0.01f &&
                lastOnGroundTime < 0)
                accelRate = 0;

            float speedDif = targetSpeed - myBody.linearVelocity.x;
            float movement = speedDif * accelRate;

            myBody.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }

        #endregion

        #region DASH

        /**
         * @brief Coroutine wykonująca ruch dash (przyspieszenie w poziomie).
         */
        private IEnumerator dash()
        {
            canDash = false;
            isDashing = true;

            SoundManager.Instance?.PlayDash();

            float oriGrav = myBody.gravityScale;
            myBody.gravityScale = 0f;

            myBody.linearVelocity = new Vector2(transform.localScale.x * data.dashPower, 0f);
            yield return new WaitForSeconds(data.dashingTime);

            myBody.linearVelocity = new Vector2(move.x * data.runMaxSpeed, myBody.linearVelocity.y);
            myBody.gravityScale = oriGrav;

            isDashing = false;
            yield return new WaitForSeconds(data.dashingCoolDown);
            canDash = true;
        }

        #endregion

        #region JUMP

        /**
         * @brief Obsługuje skakanie postaci.
         */
        private void jump()
        {
            if (grounded)
                isJumping = false;

            if (jumpButtonPressed && grounded)
            {
                isJumping = true;
                SoundManager.Instance?.PlayJump();
                myBody.linearVelocity = new Vector2(myBody.linearVelocity.x, data.jumpHeight);
            }
        }

        #endregion

        #region WALL SLIDING & JUMP

        /**
         * @brief Sprawdza i obsługuje logikę zsuwania się po ścianie.
         */
        private void WallSlidngMechanic()
        {
            if (!wallSlidingEnabled)
            {
                wallSliding = false;
                return;
            }

            Vector2 checkDir = move.x > 0 ? Vector2.right : Vector2.left;
            wall = Physics2D.Raycast(WallCheck.position, checkDir, data.wallDistance, wallLayer);

            Debug.DrawRay(WallCheck.position, new Vector2(data.wallDistance, 0f), Color.red);

            if (!grounded && wall)
            {
                wallSliding = true;
                jumpTime = Time.time + data.wallJumpTime;
            }
            else if (jumpTime < Time.time)
                wallSliding = false;
            else
                wallSliding = false;

            if (wallSliding)
                myBody.linearVelocity = new Vector2(myBody.linearVelocity.x, Mathf.Clamp(myBody.linearVelocity.y, -data.wallSlideSpeed, float.MaxValue));
        }

        /**
         * @brief Coroutine obsługująca wall jump (odbicie od ściany).
         */
        private IEnumerator wallJumpMechanic()
        {
            wallJump = true;

            if (transform.localScale.x == -3f)
                myBody.linearVelocity = new Vector2(data.wallJumpingXPower, data.wallJumpingYPower);
            else
                myBody.linearVelocity = new Vector2(-data.wallJumpingXPower, data.wallJumpingYPower);

            yield return new WaitForSeconds(data.WallJumpTimeInSecond);
            wallJump = false;
        }

        #endregion

        #region OTHER

        /**
         * @brief Zmienia kierunek, w którym patrzy postać.
         * @param isMovingRight Czy postać ma patrzeć w prawo.
         */
        private void CheckDirectionToFace(bool isMovingRight)
        {
            Vector3 scale = transform.localScale;
            scale.x = isMovingRight ? 3f : -3f;
            transform.localScale = scale;
        }

        #endregion
    }
}

