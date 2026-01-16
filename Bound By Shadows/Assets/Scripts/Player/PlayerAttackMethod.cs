using System.Collections.Generic;
using UnityEngine;

/**
 * Skrypt dodany do projektu z Asset Store z paczki EthanTheHero (autor: Twelve).
 * Odpowiada za podstawowy system ataku gracza, w tym 3-atakowe combo,
 * obsługę animacji ataku oraz przesunięcie postaci podczas wykonywania ciosów.
 *
 * Modyfikacje wprowadzone w projekcie:
 * - dostosowanie obsługi wejścia (PC + mobile),
 * - dodanie blokad ataku przy otwartym UI, pauzie i innych stanach gracza,
 * - integracja z dodatkowymi systemami gry (UIStateManager, PauseMenu),
 * - drobne zmiany logiczne i porządkowe w kodzie.
 *
 * @author Twelve (oryginał), modyfikacje: Filip Kudła
 */

namespace EthanTheHero
{

    public class PlayerAttackMethod : MonoBehaviour
    {
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

        // Flaga dla mobile
        private bool attackRequested = false;

        // Parametry animatora
        private const string attack01 = "Attack01";
        private const string attack02 = "Attack02";
        private const string attack03 = "Attack03";
        private const string notAttacking = "NotAttacking";

        /**
         * @brief Zwraca true, jeśli gracz jest w trakcie dowolnej animacji ataku.
         * Wykorzystuje zdefiniowane stałe nazwy animacji.
         */
        public bool IsAttacking
        {
            get
            {
                // Pobieramy informacje o obecnym stanie warstwy 0
                var stateInfo = myAnim.GetCurrentAnimatorStateInfo(0);

                // Sprawdzamy, czy obecna animacja to którykolwiek z ataków
                return stateInfo.IsName(attack01) ||
                       stateInfo.IsName(attack02) ||
                       stateInfo.IsName(attack03);
            }
        }

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
            if (UIStateManager.isUIOpen || playerMv.isDashing || playerMv.wallJump || playerMv.wallSliding || PauseMenu.isPaused)
                return;

            BasicAttackCombo();
        }

        /**
         * @brief Porusza postać zgodnie z atakiem (tzw. "lunge").
         */
        void FixedUpdate()
        {
            if (UIStateManager.isUIOpen || playerMv.isDashing || playerMv.wallJump || playerMv.wallSliding || PauseMenu.isPaused)
                return;

            BasicAttackMethod();
        }
        public void MobileAttackInput()
        {
            attackRequested = true;
        }

        #region BASIC ATTACK

        /**
         * @brief Obsługuje sekwencję combo ataków w oparciu o czas trwania animacji.
         *
         * Jeśli gracz kliknie odpowiednio wcześnie, przechodzi do kolejnego ataku w combo.
         */
        private void BasicAttackCombo()
        {
            bool isAttackInput = false;

            // Gra odpalona na telefonie (Android lub iOS)
            if (Application.platform == RuntimePlatform.Android)
            {
                // na telefonie tylko flaga z przycisku
                isAttackInput = attackRequested;
            }
            else
            {
                // Na PC/Edytorze liczy się Myszka LUB przycisk UI (do testów)
                isAttackInput = Input.GetMouseButtonDown(0) || attackRequested;
            }

            if (isAttackInput && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01")
                && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02")
                && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack03") && playerMv.grounded)
                {
                    myAnim.SetTrigger(attack01);
                }

            // Przejście: Attack01 → Attack02
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
            {
                if (isAttackInput)
                    atkButtonClickedOnAtk01 = true;

                if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f && atkButtonClickedOnAtk01)
                {
                    myAnim.SetTrigger(attack02);
                    atkButtonClickedOnAtk01 = false;
                }
                else if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !atkButtonClickedOnAtk01)
                {
                    myAnim.SetTrigger(notAttacking);
                }
            }

            // Przejście: Attack02 → Attack03
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
            {
                if (isAttackInput)
                    atkButtonClickedOnAtk02 = true;

                if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f && atkButtonClickedOnAtk02)
                {
                    myAnim.SetTrigger(attack03);
                    atkButtonClickedOnAtk02 = false;
                }
                else if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !atkButtonClickedOnAtk02)
                {
                    myAnim.SetTrigger(notAttacking);
                }
            }

            // Zakończenie: Attack03 → Attack01 (pętla combo)
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
            {
                if (isAttackInput)
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
            attackRequested = false;
        }

        /**
         * @brief Dodaje impuls ruchu do gracza podczas każdego z trzech ataków.
         */
        private void BasicAttackMethod()
        {
            if (transform.localScale.x > 0)
            {
                if (myAnim.GetCurrentAnimatorStateInfo(0).IsName(attack01))
                    myBody.linearVelocity = new Vector2(basicAttack01Power, myBody.linearVelocity.y);
                if (myAnim.GetCurrentAnimatorStateInfo(0).IsName(attack02))
                    myBody.linearVelocity = new Vector2(basicAttack02Power, myBody.linearVelocity.y);
                if (myAnim.GetCurrentAnimatorStateInfo(0).IsName(attack03))
                    myBody.linearVelocity = new Vector2(basicAttack03Power, myBody.linearVelocity.y);
            }
            else
            {
                if (myAnim.GetCurrentAnimatorStateInfo(0).IsName(attack01))
                    myBody.linearVelocity = new Vector2(-basicAttack01Power, myBody.linearVelocity.y);
                if (myAnim.GetCurrentAnimatorStateInfo(0).IsName(attack02))
                    myBody.linearVelocity = new Vector2(-basicAttack02Power, myBody.linearVelocity.y);
                if (myAnim.GetCurrentAnimatorStateInfo(0).IsName(attack03))
                    myBody.linearVelocity = new Vector2(-basicAttack03Power, myBody.linearVelocity.y);
            }
        }

        #endregion

        #region SOUND
        /**
         *  Metody publiczne do wykorzystania jako trigger w animacji
         */
        public void LightAttackSound()
        {
            SoundLibrary.Instance.PlayLightAttack();
        }
        public void HeavyAttackSound()
        {
            SoundLibrary.Instance.PlayHeavyAttack();
        }

        #endregion
    }
}

