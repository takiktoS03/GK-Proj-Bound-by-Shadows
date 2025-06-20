﻿using System.Collections;
using UnityEngine;


namespace EthanTheHero
{
    public class PlayerMovement : MonoBehaviour
    {
        #region FIELD

        [SerializeField] private PlayerMovementData data;
        [SerializeField] private float lastOnGroundTime;
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private Transform WallCheck;

        [HideInInspector] public Vector2 move;

        private Rigidbody2D myBody;
        private Animator myAnim;

        //Dash
        [HideInInspector] public bool isDashing;
        private bool canDash = true;
        private bool dashButtonPressed;

        //Jump
        [HideInInspector] public bool grounded;
        [HideInInspector] public bool isJumping;
        private bool jumpButtonPressed;

        //Wall Sliding and Wall Jump
        [HideInInspector] public bool wallJump;
        [HideInInspector] public bool wallSliding;
        private RaycastHit2D wall;
        private float jumpTime;

        //[SerializeField] private AudioClip runAudioClip;

        //private AudioSource runAudioSource;
        private PlayerHealth healthComponent;
        private bool stepSoundPlaying = false;

        private IEnumerator ResetStepSound()
        {
            yield return new WaitForSeconds(0.3f); // dostosuj do długości kroku
            stepSoundPlaying = false;
        }


        #endregion

        #region MONOBEHAVIOUR
        void Awake()
        {
            myBody = GetComponent<Rigidbody2D>();
            myAnim = GetComponent<Animator>();
            healthComponent = GetComponent<PlayerHealth>();

            // Dodajemy AudioSource i przypisujemy mu AudioClip
            //runAudioSource = gameObject.AddComponent<AudioSource>();
            //runAudioSource.clip = runAudioClip;
            //runAudioSource.loop = true; // zapętlony dźwięk biegania
        }

        void Update()
        {
            if (isDashing || wallJump || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01") || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02") || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
                return;

            lastOnGroundTime -= Time.deltaTime;

            //Input Handler
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
            if (isDashing || wallJump || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01") || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02") || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
                return;

            if (!wallSliding)
                run(1);

            // running
            if (move.x != 0 && grounded)
            {
                SoundManager.Instance?.StartSteps();
            }
            else
            {
                SoundManager.Instance?.StopSteps();
            }


            //checks if set box overlaps with ground
            if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer))
            {
                lastOnGroundTime = 0.1f;
                grounded = true;
            }
            else
                grounded = false;


            WallSlidngMechanic();
        }
        #endregion

        #region RUN
        private void run(float lerpAmount)
        {
            float targetSpeed = move.x * data.runMaxSpeed;

            float accelRate;

            targetSpeed = Mathf.Lerp(myBody.linearVelocity.x, targetSpeed, lerpAmount);

            //Calculate Acceleration and Decceleration
            if (lastOnGroundTime > 0)
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount : data.runDeccelAmount;
            else
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount * data.accelInAir : data.runDeccelAmount * data.deccelInAir;

            if (data.doConserveMomentum && Mathf.Abs(myBody.linearVelocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(myBody.linearVelocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && lastOnGroundTime < 0)
                accelRate = 0;

            float speedDif = targetSpeed - myBody.linearVelocity.x;
            float movement = speedDif * accelRate;

            //Implementing run
            myBody.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }
        #endregion

        #region DASH

        private IEnumerator dash()
        {
            canDash = false;
            isDashing = true;

            //sound
            SoundManager.Instance?.PlayDash();

            float oriGrav = myBody.gravityScale;
            myBody.gravityScale = 0f;

            myBody.linearVelocity = new Vector2(transform.localScale.x * data.dashPower, 0f);
            yield return new WaitForSeconds(data.dashingTime);
            if (move.x > 0)
            {
                myBody.linearVelocity = new Vector2(data.runMaxSpeed, myBody.linearVelocity.y);
            }
            else if (move.x < 0)
            {
                myBody.linearVelocity = new Vector2(-data.runMaxSpeed, myBody.linearVelocity.y);
            }
            myBody.gravityScale = oriGrav;

            isDashing = false;
            yield return new WaitForSeconds(data.dashingCoolDown);
            canDash = true;



        }
        #endregion

        #region JUMP
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

        #region Wall Sliding and Wall Jump
        private void WallSlidngMechanic()
        {
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
            Vector3 tem = transform.localScale;
            if (!isMovingRight)
                tem.x = -3f;
            else
                tem.x = 3f;
            transform.localScale = tem;
        }
        #endregion
    }
}
