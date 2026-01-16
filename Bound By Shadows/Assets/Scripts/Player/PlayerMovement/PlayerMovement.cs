using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Skrypt dodany do projektu z Asset Store z paczki EthanTheHero (autor: Twelve).
 * Odpowiada za pełną logikę ruchu gracza, w tym poruszanie się,
 * skok, dash, wall slide, wall jump oraz interakcję z podłożem.
 *
 * Modyfikacje wprowadzone w projekcie:
 * - rozszerzenie obsługi sterowania o urządzenia mobilne (joystick),
 * - integracja systemu staminy i kosztów dasha,
 * - dodanie obsługi nachylonych powierzchni (slopes),
 * - rozbudowana detekcja podłoża i ścian,
 * - integracja z systemami UI, animacji i dźwięku,
 * - uporządkowanie oraz rozbudowa logiki ruchu.
 *
 * @author Twelve (oryginał), modyfikacje: Julia Bigaj
 */

namespace EthanTheHero
{
    public class PlayerMovement : MonoBehaviour
    {
        #region FIELD
        [SerializeField] private PlayerMovementData data;
        [SerializeField] private float lastOnGroundTime;
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private Vector2 groundCheckSize = new(0.49f, 0.03f);
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private Transform WallCheck;

        [Header("Mobile Controls")]
        public Joystick joystick;
        [Range(0.1f, 1.0f)]
        public float jumpThreshold = 0.5f;

        private bool mobileJumpRequest; // Flaga dla przycisku skoku
        private bool mobileDashRequest; // Flaga dla przycisku dasha

        [HideInInspector] public Vector2 move;

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

        // Wall
        [HideInInspector] public bool wallSlidingEnabled = true;
        [HideInInspector] public bool wallJump;
        [HideInInspector] public bool wallSliding;
        private RaycastHit2D wall;
        private float jumpTime;

        private PlayerHealth healthComponent;

        // == informacje o podłożu / stokach ===
        [Header("Slopes")]
        [SerializeField] private float maxSlopeAngle = 60f;
        [SerializeField] private float slopeSlideMultiplier = 3f;

        private Vector2 groundNormal = Vector2.up;
        private float slopeAngle = 0f;
        private bool hasGroundSupport = false;

        private Vector3 lastPosition;
        private float currentSpeed;
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

            // === INPUT (PC + MOBILE) ===
            float inputX = Input.GetAxisRaw("Horizontal");
            dashButtonPressed = Input.GetKeyDown(KeyCode.W) || mobileDashRequest;
            mobileDashRequest = false;

            jumpButtonPressed = Input.GetButtonDown("Jump") || mobileJumpRequest;
            mobileJumpRequest = false;

            // Ruch (Joystick - nadpisuje klawiaturę, jeśli jest używany)
            if (joystick != null)
            {
                if (Mathf.Abs(joystick.Horizontal) > 0.1f)
                {
                    inputX = joystick.Horizontal;
                }
                if (joystick.Vertical > jumpThreshold)
                {
                    mobileJumpRequest = true;
                }

            }
            move.x = inputX;

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

            bool boxGrounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);

            GetGroundInfo();
            grounded = boxGrounded || hasGroundSupport;

            if (grounded)
            {
                lastOnGroundTime = 0.1f;
            }

            myBody.gravityScale = 1f;

            if (!wallSliding)
                run(1);

            HandleSlopeSliding();

            if (grounded && Mathf.Abs(move.x) > 0.1f) SoundLibrary.Instance.StartSteps();
            else SoundLibrary.Instance.StopSteps();

            WallSlidngMechanic();

            Vector2 worldVel = (transform.position - lastPosition) / Time.fixedDeltaTime;
            lastPosition = transform.position;
            Vector2 tangent = GetSlopeTangent();
            currentSpeed = Mathf.Abs(Vector2.Dot(worldVel, tangent));
        }
        #endregion

        #region MOBILE METHODS

        public void MobileDashInput()
        {
            mobileDashRequest = true;
        }
        #endregion

        #region RUN
        private void run(float lerpAmount)
        {
            float targetSpeed = move.x * data.runMaxSpeed;
            float accelRate;

            targetSpeed = Mathf.Lerp(myBody.linearVelocity.x, targetSpeed, lerpAmount);

            if (lastOnGroundTime > 0)
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount : data.runDeccelAmount;
            else
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount * data.accelInAir : data.runDeccelAmount * data.deccelInAir;

            float speedDif = targetSpeed - myBody.linearVelocity.x;
            float movement = speedDif * accelRate;

            Vector2 moveDir;

            if (grounded && slopeAngle < maxSlopeAngle)
                moveDir = GetSlopeTangent();
            else
                moveDir = Vector2.right;

            myBody.AddForce(moveDir * movement, ForceMode2D.Force);
        }
        #endregion

        #region DASH
        private IEnumerator dash()
        {
            canDash = false;
            isDashing = true;

            SoundLibrary.Instance.PlayDash();

            float originalGravity = myBody.gravityScale;
            myBody.gravityScale = 0f;

            Vector2 dashDir = new Vector2(transform.localScale.x, 0f);
            myBody.linearVelocity = dashDir * data.dashPower;

            yield return new WaitForSeconds(data.dashingTime);

            myBody.linearVelocity = Vector2.zero;
            myBody.gravityScale = originalGravity;
            isDashing = false;

            yield return new WaitForSeconds(data.dashingCoolDown);
            canDash = true;
        }
        #endregion
        #region JUMP
        private void jump()
        {
            if (grounded) isJumping = false;

            if (jumpButtonPressed && grounded)
            {
                isJumping = true;
                SoundLibrary.Instance.PlayJump();
                myBody.linearVelocity = new Vector2(myBody.linearVelocity.x, data.jumpHeight);
            }
        }
        #endregion

        #region WALL
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
                myBody.linearVelocity = new Vector2(
                    myBody.linearVelocity.x,
                    Mathf.Clamp(myBody.linearVelocity.y, -data.wallSlideSpeed, float.MaxValue)
                );
        }

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
        private void CheckDirectionToFace(bool isMovingRight)
        {
            Vector3 scale = transform.localScale;
            scale.x = isMovingRight ? 3f : -3f;
            transform.localScale = scale;
        }

        private void GetGroundInfo()
        {
            Vector2 capsuleSize = new Vector2(groundCheckSize.x, Mathf.Max(groundCheckSize.y, 0.10f));
            float castDist = 0.35f;

            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(groundLayer);
            filter.useTriggers = false;

            RaycastHit2D[] hits = new RaycastHit2D[8];
            int hitCount = Physics2D.CapsuleCast(
                groundCheckPoint.position, capsuleSize,
                CapsuleDirection2D.Horizontal, 0f,
                Vector2.down, filter, hits, castDist
            );

            hasGroundSupport = false;

            if (hitCount > 0)
            {
                Vector2 sum = Vector2.zero; int n = 0;
                for (int i = 0; i < hitCount; i++)
                {
                    var h = hits[i];
                    if (!h.collider) continue;
                    if (h.normal.y < 0.05f) continue;
                    sum += h.normal; n++;
                }
                if (n > 0)
                {
                    groundNormal = (sum / n).normalized;
                    slopeAngle = Vector2.Angle(groundNormal, Vector2.up);
                    hasGroundSupport = true;
                    return;
                }
            }
            groundNormal = Vector2.up;
            slopeAngle = 0f;
        }

        private Vector2 GetSlopeTangent() => new Vector2(groundNormal.y, -groundNormal.x).normalized;

        private void HandleSlopeSliding()
        {
            if (!grounded) return;

            if (slopeAngle > maxSlopeAngle)
            {
                Vector2 tangent = GetSlopeTangent();
                float slideForce = (slopeAngle - maxSlopeAngle) / 90f * slopeSlideMultiplier;
                myBody.AddForce(-tangent * slideForce, ForceMode2D.Force);
            }
        }
        public void ResetAfterLoad()
        {
            isDashing = false;
            wallJump = false;
            wallSliding = false;

            canDash = true;

            grounded = true;
            lastOnGroundTime = 0.1f;

            myBody.simulated = true;
            myBody.bodyType = RigidbodyType2D.Dynamic;
            myBody.constraints = RigidbodyConstraints2D.FreezeRotation;

            myBody.linearVelocity = Vector2.zero;
            myBody.angularVelocity = 0f;

            if (myAnim != null)
            {
                myAnim.Rebind();
                myAnim.Update(0f);
            }
        }

        #endregion
    }
}
