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

        /////////
        [SerializeField] private float maxSlopeAngle = 60f;       // max kąt, który traktujemy jako "chodzalny"
        [SerializeField] private float slopeSlideMultiplier = 3f; // siła zsuwania po zbyt stromym zboczu
        private Vector2 groundNormal = Vector2.up;
        //private bool isOnSlope = false;
        private float slopeAngle = 0f;
        [Header("Slope Uphill Speed")]
        [SerializeField] private float uphillStartFactor = 0.5f;    // ile % maks prędkości ma być na starcie (0.0..1.0)
        [SerializeField] private float uphillGainPerSecond = 1.0f; // jak szybko progress rośnie (1 = 1/s)
        private float uphillProgress = 0f;
        [SerializeField] private float coyoteTime = 0.1f; // 100 ms na spóźniony skok


        [Tooltip("Ile ponad normalną runMaxSpeed można dojść przy długim podchodzeniu (1.0 = bez bonusa).")]
        [SerializeField] private float uphillMaxBoost = 1.25f;

        [Tooltip("Premia do przyspieszenia, gdy faktycznie idziemy pod górę.")]
        [SerializeField] private float uphillAccelBonus = 1.5f;

        [SerializeField] private float jumpBufferTime = 0.12f; // input bufor (ms)
        private float lastPressedJumpTime = 0f;


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

            // najpierw zbierz info o podłodze
            grounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);
            GetGroundInfo();

            // coyote time w fizyce
            if (grounded) lastOnGroundTime = coyoteTime;
            else lastOnGroundTime = Mathf.Max(0f, lastOnGroundTime - Time.fixedDeltaTime);

            // uruchom ruch po pochylni jeśli nie wall sliding
            if (!wallSliding)
                run(1);

            HandleSlopeSliding();

            // kroki (jak wcześniej)
            if (move.x != 0 && grounded)
                SoundManager.Instance?.StartSteps();
            else
                SoundManager.Instance?.StopSteps();

            // — PODMIANA W FixedUpdate —
            if (grounded && slopeAngle > 0.1f && slopeAngle <= maxSlopeAngle && IsMovingUphill() && !isJumping)
            {
                uphillProgress += uphillGainPerSecond * Time.fixedDeltaTime;
            }
            else
            {
                uphillProgress -= uphillGainPerSecond * Time.fixedDeltaTime;
            }

            // coyote time
            if (grounded) lastOnGroundTime = coyoteTime;

            uphillProgress = Mathf.Clamp01(uphillProgress);

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
            // Jeśli na pochylni i nie skaczemy - poruszamy się wzdłuż stycznej (zachowując fizykę)
            // — PODMIANA W run(float) —
            if (grounded && slopeAngle > 0.1f && slopeAngle <= maxSlopeAngle && !isJumping)
            {
                Vector2 tangent = GetSlopeTangent();  // znormalizowana styczna
                Debug.DrawLine(groundCheckPoint.position, groundCheckPoint.position + (Vector3)tangent * 0.8f, Color.green);

                // 1) limit prędkości rosnący z czasem podchodzenia
                float startSpeed = data.runMaxSpeed * uphillStartFactor;
                float targetMax = Mathf.Lerp(startSpeed, data.runMaxSpeed * uphillMaxBoost, uphillProgress);

                // 2) znak wejścia względem stycznej
                float tangentSignForRight = Mathf.Sign(Vector2.Dot(tangent, Vector2.right));
                float inputAlongTangent = Mathf.Clamp(move.x, -1f, 1f) * tangentSignForRight;

                // 3) docelowa prędkość skalarna wzdłuż stycznej
                float targetScalar = inputAlongTangent * targetMax;

                // 4) aktualna prędkość skalarna wzdłuż stycznej
                float currentScalar = Vector2.Dot(myBody.linearVelocity, tangent);

                // 5) tempo „dowiązywania” (jak przyspieszenie)
                float accelRate;
                if (lastOnGroundTime > 0)
                    accelRate = (Mathf.Abs(targetScalar) > 0.01f) ? data.runAccelAmount : data.runDeccelAmount;
                else
                    accelRate = (Mathf.Abs(targetScalar) > 0.01f) ? data.runAccelAmount * data.accelInAir : data.runDeccelAmount * data.deccelInAir;

                if (slopeAngle <= maxSlopeAngle && IsMovingUphill())
                    accelRate *= uphillAccelBonus;

                // 6) płynne dojście do celu na prędkości (nie AddForce)
                float maxDelta = accelRate * Time.fixedDeltaTime;
                float newScalar = Mathf.MoveTowards(currentScalar, targetScalar, maxDelta);

                // 7) zachowaj składową normalną (kontakt z ziemią)
                Vector2 v = myBody.linearVelocity;
                float vn = Vector2.Dot(v, groundNormal);
                Vector2 newVel = tangent * newScalar + groundNormal * vn;

                myBody.linearVelocity = newVel;
                return;
            }

            // fallback: logika ruchu na płasko / w powietrzu (z AddForce)
            float targetSpeed = move.x * data.runMaxSpeed;
            float accel;

            float desiredSpeedX = Mathf.Lerp(myBody.linearVelocity.x, targetSpeed, lerpAmount);

            if (lastOnGroundTime > 0)
                accel = (Mathf.Abs(desiredSpeedX) > 0.01f) ? data.runAccelAmount : data.runDeccelAmount;
            else
                accel = (Mathf.Abs(desiredSpeedX) > 0.01f) ? data.runAccelAmount * data.accelInAir : data.runDeccelAmount * data.deccelInAir;

            if (data.doConserveMomentum &&
                Mathf.Abs(myBody.linearVelocity.x) > Mathf.Abs(desiredSpeedX) &&
                Mathf.Sign(myBody.linearVelocity.x) == Mathf.Sign(desiredSpeedX) &&
                Mathf.Abs(desiredSpeedX) > 0.01f &&
                lastOnGroundTime < 0)
                accel = 0;

            float speedDifference = desiredSpeedX - myBody.linearVelocity.x;
            float movementForce = speedDifference * accel;

            myBody.AddForce(movementForce * Vector2.right, ForceMode2D.Force);
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
            // coyote-time + real grounded
            bool canJump = grounded || lastOnGroundTime > 0f;

            if (canJump && jumpButtonPressed)
            {
                isJumping = true;
                SoundManager.Instance?.PlayJump();

                // kierunek skoku: na stoku – normalna podłoża, na płaskim – do góry
                Vector2 jumpDir = (slopeAngle > 0.1f && slopeAngle <= maxSlopeAngle) ? groundNormal : Vector2.up;
                jumpDir.Normalize();

                // rozbij obecną prędkość na styczną i normalną, usuń wchodzenie w grunt
                Vector2 v = myBody.linearVelocity; // lub myBody.velocity
                                                   // styczna do aktualnej normalnej
                Vector2 tangent = new Vector2(jumpDir.y, -jumpDir.x);
                float vT = Vector2.Dot(v, tangent);
                float vN = Vector2.Dot(v, jumpDir);
                if (vN < 0f) v -= jumpDir * vN; // kasuj wbijanie w ziemię

                // ustaw nową prędkość: zachowaj ruch wzdłuż stycznej, dodaj impuls skoku po normalnej
                myBody.linearVelocity = tangent * vT + jumpDir * data.jumpHeight;

                // wyczyść coyote timer po skoku
                lastOnGroundTime = 0f;
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
            RaycastHit2D hit = Physics2D.Raycast(WallCheck.position, checkDir, data.wallDistance, wallLayer);

            // ignoruj hity, które mają znaczący komponent Y (to nie jest prawdziwa ściana)
            if (hit.collider && hit.normal.y > 0.3f)
            {
                // traktujemy to jak "ground" — nie uruchamiamy wallSliding
                hit = default;
            }

            wall = hit;

            if (!grounded && wall && Mathf.Abs(wall.normal.x) > 0.5f)
            {
                wallSliding = true;
                jumpTime = Time.time + data.wallJumpTime;
            }
            else if (jumpTime < Time.time)
            {
                wallSliding = false;
            }

            if (wallSliding)
                myBody.linearVelocity = new Vector2(myBody.linearVelocity.x, Mathf.Clamp(myBody.linearVelocity.y, -data.wallSlideSpeed, float.MaxValue));
        }

        /**
         * @brief Coroutine obsługująca wall jump (odbicie od ściany).
         */
        private IEnumerator wallJumpMechanic()
        {
            wallJump = true;

            Vector2 jumpDir = Vector2.up + (transform.localScale.x == -3f ? Vector2.right : Vector2.left) * 0.7f;
            jumpDir.Normalize();

            //if (transform.localScale.x == -3f)
            //    myBody.linearVelocity = new Vector2(data.wallJumpingXPower, data.wallJumpingYPower);
            //else
            //myBody.linearVelocity = new Vector2(-data.wallJumpingXPower, data.wallJumpingYPower);

            myBody.linearVelocity = jumpDir * data.wallJumpingYPower;

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
        private void GetGroundInfo()
        {
            // trzy punkty próbkujące pod stopami
            float halfW = groundCheckSize.x * 0.5f;
            Vector2 originC = groundCheckPoint.position;
            Vector2 originL = originC + new Vector2(-halfW, 0f);
            Vector2 originR = originC + new Vector2(+halfW, 0f);

            float dist = 1.2f; // zasięg raycastów
            int layer = groundLayer;

            RaycastHit2D hitC = Physics2D.Raycast(originC, Vector2.down, dist, layer);
            RaycastHit2D hitL = Physics2D.Raycast(originL, Vector2.down, dist, layer);
            RaycastHit2D hitR = Physics2D.Raycast(originR, Vector2.down, dist, layer);

            // Debug helpery
            Debug.DrawRay(originC, Vector2.down * dist, Color.yellow);
            Debug.DrawRay(originL, Vector2.down * dist, Color.yellow);
            Debug.DrawRay(originR, Vector2.down * dist, Color.yellow);

            // zbierz ważne trafienia
            Vector2 sumNormals = Vector2.zero;
            int n = 0;

            void AddIfValid(RaycastHit2D h)
            {
                if (h.collider == null) return;
                // ignoruj „prawie płaskie” ściany (jeśli trafi bok kafla)
                if (h.normal.y < 0.1f) return;
                sumNormals += h.normal;
                n++;
            }

            AddIfValid(hitC);
            AddIfValid(hitL);
            AddIfValid(hitR);

            if (n > 0)
            {
                groundNormal = (sumNormals / n).normalized;
                slopeAngle = Vector2.Angle(groundNormal, Vector2.up);
                //isOnSlope = slopeAngle > 0.1f && slopeAngle <= maxSlopeAngle;
            }
            else
            {
                groundNormal = Vector2.up;
                slopeAngle = 0f;
                //isOnSlope = false;
            }
        }

        private Vector2 GetSlopeTangent()
        {
            // tangent pointing to the right relative to normal
            return new Vector2(groundNormal.y, -groundNormal.x).normalized;
        }

        private bool IsMovingUphill()
        {
            if (Mathf.Approximately(move.x, 0f)) return false;
            Vector2 tangent = GetSlopeTangent();
            // Jeżeli składowa pionowa stycznej ma ten sam znak co wejście (move.x),
            // to poruszamy się "w górę" stycznej (uplhill). 
            // (tangent.y * move.x) > 0 oznacza, że poruszanie w prawo/left idzie w górę.
            return (tangent.y * move.x) > 0f;
        }


        private void HandleSlopeSliding()
        {
            // jeśli trafiliśmy na zbyt stromą pochyłość (> maxSlopeAngle) i mamy bardzo mały input, zsuwamy się
            if (!grounded) return;

            // normalnie isOnSlope = true tylko dla kątów <= maxSlopeAngle; jeżeli slopeAngle > max, to zsuwaj
            if (slopeAngle > maxSlopeAngle)
            {
                Vector2 tangent = GetSlopeTangent();
                // kierunek zsuwania powinien być "w dół" stycznej => -tangent jeśli tangent wskazuje w górę
                // uproszczenie: zastosuj siłę proporcjonalną do kąta w kierunku -tangent
                float slideForce = (slopeAngle - maxSlopeAngle) / 90f * slopeSlideMultiplier;
                myBody.AddForce(-tangent * slideForce, ForceMode2D.Force);
            }

            else
            {
                // lekki efekt zsuwania, jeśli brak inputu i kąt umiarkowany (opcjonalnie)
                if (Mathf.Approximately(move.x, 0f) && slopeAngle > 1f)
                {
                    Vector2 tangent = GetSlopeTangent();
                    float tinySlide = (slopeAngle / maxSlopeAngle) * 0.2f;
                    myBody.AddForce(-tangent * tinySlide, ForceMode2D.Force);
                    //Debug.Log($"slopeAngle={slopeAngle:F1} isOnSlope={isOnSlope} move.x={move.x} uphillProgress={uphillProgress:F2} currentScalar={Vector2.Dot(myBody.linearVelocity, tangent):F2}");

                }
            }
        }
        #endregion
    }
}

