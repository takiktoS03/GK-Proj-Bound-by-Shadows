using System.Collections.Generic;
using UnityEngine;
using static UIStateManager;

namespace EthanTheHero
{
    /**
     * @class PlayerAttackMethod
     * @brief Obsługuje podstawowy system ataku postaci gracza (combo 3-atakowe).
     *
     * Umożliwia wykonywanie sekwencyjnych ataków (combo), reagując na kliknięcia myszy
     * w odpowiednim czasie trwania animacji. Obsługuje również dźwięki i przesunięcia gracza
     * podczas ataku.
     *
     * Używa animatora oraz komponentów PlayerMovement i Rigidbody2D.
     */
    public class PlayerAttackMethod : MonoBehaviour
    {
        /// @brief Czy gra jest zapauzowana (globalnie).
        public static bool isPaused = false;

        #region FIELD

        private PlayerAnimation playerAnim;
        private PlayerMovement playerMv;
        private Animator myAnim;
        private Rigidbody2D myBody;

        [Header("Basic Attack")]
        public float basicAttack01Power = 0.5f;  ///< Siła przesunięcia gracza podczas 1 ataku
        public float basicAttack02Power = 0.5f;  ///< Siła przesunięcia gracza podczas 2 ataku
        public float basicAttack03Power = 0.9f;  ///< Siła przesunięcia gracza podczas 3 ataku

        // Flagi dla combo inputów
        private bool atkButtonClickedOnAtk01;
        private bool atkButtonClickedOnAtk02;
        private bool atkButtonClickedOnAtk03;

        // Parametry animatora
        private const string attack01 = "Attack01";
        private const string attack02 = "Attack02";
        private const string attack03 = "Attack03";
        private const string notAttacking = "NotAttacking";

        // Odwołanie do SoundManagera
        private SoundManager soundManager;

        #endregion

        /**
         * @brief Inicjalizacja komponentów.
         */
        void Awake()
        {
            myAnim = GetComponent<Animator>();
            playerAnim = GetComponent<PlayerAnimation>();
            myBody = GetComponent<Rigidbody2D>();
            playerMv = GetComponent<PlayerMovement>();
            soundManager = SoundManager.Instance;
        }

        /**
         * @brief Ustawia początkową animację postaci.
         */
        void Start()
        {
            myAnim.Play("Idle");
        }

        /**
         * @brief Obsługuje logikę ataku combo (wejście i przejścia).
         */
        void Update()
        {
            if (isUIOpen || playerMv.isDashing || playerMv.wallJump || playerMv.wallSliding || PauseMenu.isPaused)
                return;

            BasicAttackCombo();
        }

        /**
         * @brief Porusza postać zgodnie z atakiem (tzw. "lunge").
         */
        void FixedUpdate()
        {
            if (isUIOpen || playerMv.isDashing || playerMv.wallJump || playerMv.wallSliding || PauseMenu.isPaused)
                return;

            BasicAttackMethod();
        }

        #region BASIC ATTACK

        /**
         * @brief Obsługuje sekwencję combo ataków w oparciu o czas trwania animacji.
         *
         * Jeśli gracz kliknie odpowiednio wcześnie, przechodzi do kolejnego ataku w combo.
         */
        private void BasicAttackCombo()
        {
            // Start combo
            if (Input.GetMouseButtonDown(0) && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01")
                && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02")
                && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack03") && playerMv.grounded)
            {
                myAnim.SetTrigger(attack01);
                soundManager?.PlayLightAttack();
            }

            // Przejście: Attack01 → Attack02
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
            {
                if (Input.GetMouseButtonDown(0))
                    atkButtonClickedOnAtk01 = true;

                if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f && atkButtonClickedOnAtk01)
                {
                    myAnim.SetTrigger(attack02);
                    atkButtonClickedOnAtk01 = false;
                    soundManager?.PlayLightAttack();
                }
                else if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !atkButtonClickedOnAtk01)
                {
                    myAnim.SetTrigger(notAttacking);
                }
            }

            // Przejście: Attack02 → Attack03
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
            {
                if (Input.GetMouseButtonDown(0))
                    atkButtonClickedOnAtk02 = true;

                if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f && atkButtonClickedOnAtk02)
                {
                    myAnim.SetTrigger(attack03);
                    atkButtonClickedOnAtk02 = false;
                    soundManager?.PlayHeavyAttack();
                }
                else if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !atkButtonClickedOnAtk02)
                {
                    myAnim.SetTrigger(notAttacking);
                }
            }

            // Zakończenie: Attack03 → Attack01 (pętla combo)
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
            {
                if (Input.GetMouseButtonDown(0))
                    atkButtonClickedOnAtk03 = true;

                if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && atkButtonClickedOnAtk03)
                {
                    myAnim.SetTrigger(attack01);
                    atkButtonClickedOnAtk03 = false;
                }
                else if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !atkButtonClickedOnAtk03)
                {
                    myAnim.SetTrigger(notAttacking);
                }
            }
        }

        /**
         * @brief Dodaje impuls ruchu do gracza podczas każdego z trzech ataków.
         */
        private void BasicAttackMethod()
        {
            if (transform.localScale.x > 0)
            {
                if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
                    myBody.linearVelocity = new Vector2(basicAttack01Power, myBody.linearVelocity.y);
                if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
                    myBody.linearVelocity = new Vector2(basicAttack02Power, myBody.linearVelocity.y);
                if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
                    myBody.linearVelocity = new Vector2(basicAttack03Power, myBody.linearVelocity.y);
            }
            else
            {
                if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
                    myBody.linearVelocity = new Vector2(-basicAttack01Power, myBody.linearVelocity.y);
                if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
                    myBody.linearVelocity = new Vector2(-basicAttack02Power, myBody.linearVelocity.y);
                if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
                    myBody.linearVelocity = new Vector2(-basicAttack03Power, myBody.linearVelocity.y);
            }
        }

        #endregion
    }
}
