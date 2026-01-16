using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Skrypt dodany do projektu z Asset Store z paczki EthanTheHero (autor: Twelve).
 * Odpowiada za sterowanie animacjami gracza na podstawie jego ruchu,
 * stanu fizyki oraz akcji takich jak bieg, skok, dash i wall slide.
 *
 * Modyfikacje wprowadzone w projekcie:
 * - dostosowanie parametrów animatora do aktualnej logiki gracza,
 * - integracja z dodatkowymi systemami (np. dash, wall jump),
 * - uporządkowanie i rozbudowa logiki animacji,
 * - usunięcie testowych wywołań animacji z klawiatury.
 *
 * @author Twelve (oryginał), modyfikacje: Filip Kudła
 */

namespace EthanTheHero
{
    public class PlayerAnimation : MonoBehaviour
    {
        #region FIELD

        private PlayerMovement playerMv;
        private Animator myAnim;
        private Rigidbody2D myBody;
        private PlayerAttackMethod playerAtt;

        // Nazwy parametrów animacji
        private const string speed = "Speed";
        private const string runIdle = "RunIdlePlayying";
        private const string jump = "Grounded";
        private const string yvelocity = "Yvelocity";
        private const string dash = "Dashing";
        private const string wallSliding = "WallSliding";
        private const string wallJump = "WallJump";
        private const string hurt = "Hurt";
        private const string hurtEnded = "HurtEnded";
        private const string death = "Death";
        private const string deathEnded = "DeathEnded";

        /// @brief Czy animacja przejścia Run→Idle jest obecnie odtwarzana.
        private bool runIdleIsPlayying;

        #endregion

        /**
         * @brief Inicjalizacja referencji do komponentów.
         */
        void Awake()
        {
            playerMv = GetComponent<PlayerMovement>();
            myAnim = GetComponent<Animator>();
            myBody = GetComponent<Rigidbody2D>();
            playerAtt = GetComponent<PlayerAttackMethod>();
        }

        /**
         * @brief Ustawia odpowiednie parametry animatora na podstawie aktualnego stanu gracza.
         */
        void Update()
        {
            #region IDLE & RUN

            myAnim.SetFloat(speed, Mathf.Abs(playerMv.move.x));

            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("RunIdleTrans"))
            {
                runIdleIsPlayying = true;
                if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    runIdleIsPlayying = false;
            }
            myAnim.SetBool(runIdle, runIdleIsPlayying);

            #endregion

            #region JUMP

            myAnim.SetBool(jump, playerMv.grounded);
            myAnim.SetFloat(yvelocity, myBody.linearVelocity.y);

            #endregion

            #region DASH

            myAnim.SetBool(dash, playerMv.isDashing);

            #endregion

            #region WALL SLIDING & WALL JUMP

            myAnim.SetBool(wallSliding, playerMv.wallSliding);
            myAnim.SetBool(wallJump, playerMv.wallJump);

            #endregion

            #region HURT & DEATH (do aktywacji ręcznej lub przez logikę zdrowia)

            /*
            // Przykładowe uruchomienie animacji obrażeń
            if (Input.GetKeyDown(KeyCode.H))
            {
                myAnim.SetTrigger(hurt);
                myBody.linearVelocity = Vector2.zero;
            }

            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Hurt") &&
                myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                myAnim.SetTrigger(hurtEnded);
            }

            // Przykładowe uruchomienie animacji śmierci
            if (Input.GetKeyDown(KeyCode.X))
            {
                myAnim.SetTrigger(death);
                myBody.linearVelocity = Vector2.zero;
            }

            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Death") &&
                myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                myAnim.SetTrigger(deathEnded);
            }
            */

            #endregion
        }
    }
}

