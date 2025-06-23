using System.Collections.Generic;
using UnityEngine;
using static UIStateManager;

namespace EthanTheHero
{
	public class PlayerAttackMethod : MonoBehaviour
	{
        public static bool isPaused = false;

        #region FIELD

        private PlayerAnimation playerAnim;
		private PlayerMovement playerMv;
		private Animator myAnim;
		private Rigidbody2D myBody;

        [Header("Basic Attack")]
		public float basicAttack01Power = 0.5f;
		public float basicAttack02Power = 0.5f;
		public float basicAttack03Power = 0.9f;

		private bool atkButtonClickedOnAtk01;
		private bool atkButtonClickedOnAtk02;
		private bool atkButtonClickedOnAtk03;

		private const string attack01 = "Attack01";
		private const string attack02 = "Attack02";
		private const string attack03 = "Attack03";
		private const string notAttacking = "NotAttacking";

		// dzwiek
        private SoundManager soundManager;

        #endregion

        void Awake()
		{
			myAnim = GetComponent<Animator>();
			playerAnim = GetComponent<PlayerAnimation>();
			myBody = GetComponent<Rigidbody2D>();
			playerMv = GetComponent<PlayerMovement>();
            soundManager = SoundManager.Instance;
        }

        void Start()
        {
            myAnim.Play("Idle");
        }

        void Update()
		{
            if (isUIOpen || playerMv.isDashing || playerMv.wallJump || playerMv.wallSliding || PauseMenu.isPaused)
				return;

			BasicAttackCombo();
		}

		void FixedUpdate()
		{
			if (isUIOpen || playerMv.isDashing || playerMv.wallJump || playerMv.wallSliding || PauseMenu.isPaused)
				return;

			BasicAttackMethod();
		}

		#region BASIC ATTACK

		private void BasicAttackCombo()
		{
			//Combo attack mechanic
			if (Input.GetMouseButtonDown(0) && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01") && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02") && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack03") && playerMv.grounded)
            {
                myAnim.SetTrigger(attack01);
                soundManager?.PlayLightAttack(); // ✅ odpalany tylko przy rozpoczęciu ataku
            }


            //Set combo attack 01 
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
			{
				//See if attak button is clicked
				if (Input.GetMouseButtonDown(0))
					atkButtonClickedOnAtk01 = true;

				//Set if attack 01 animation is ended playying and attack button is clicked while attack 01 animation is playing
				if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8 && atkButtonClickedOnAtk01)
				{
					myAnim.SetTrigger(attack02);
					atkButtonClickedOnAtk01 = false;
					soundManager?.PlayLightAttack();

				}
				//Set if attack 01 animation is ended playying and attack button is not clicked while attack 01 animation is playing
				else if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !atkButtonClickedOnAtk01)
					myAnim.SetTrigger(notAttacking);
			}

			//Set combo attack 02
			if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
			{
				//See if attak button is clicked
				if (Input.GetMouseButtonDown(0))
					atkButtonClickedOnAtk02 = true;

				//Set if attack 02 animation is ended playying and attack button is clicked while attack 02 animation is playing
				if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8 && atkButtonClickedOnAtk02)
				{
					myAnim.SetTrigger(attack03);
					atkButtonClickedOnAtk02 = false;
					soundManager?.PlayHeavyAttack();
				}
				//Set if attack 02 animation is ended playying and attack button is not clicked while attack 02 animation is playing
				else if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !atkButtonClickedOnAtk02)
					myAnim.SetTrigger(notAttacking);
			}

			//Set combo attack 03
			if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
			{
				//See if attak button is clicked
				if (Input.GetMouseButtonDown(0))
					atkButtonClickedOnAtk03 = true;

				//Set if attack 03 animation is ended playying and attack button is clicked while attack 03 animation is playing
				if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && atkButtonClickedOnAtk03)
				{
					myAnim.SetTrigger(attack01);
					atkButtonClickedOnAtk03 = false;

				}
				//Set if attack 03 animation is ended playying and attack button is not clicked while attack 03 animation is playing
				else if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !atkButtonClickedOnAtk03)
					myAnim.SetTrigger(notAttacking);
			}
		}

		private void BasicAttackMethod()
		{
			//Move player if player is in attacking state
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
