using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovementGrappling : MonoBehaviour
{
        [Header("Movement")]
        private float moveSpeed;
        public float walkSpeed;

        public float groundDrag;

        [Header("Jumping")]
        public float jumpForce;
        public float jumpCooldown;
        public float airMultiplier;
        bool readyToJump;

        [Header("Keybinds")]
        public KeyCode jumpKey = KeyCode.Space;

        [Header("Ground Check")]
        public float playerHeight;
        public LayerMask whatIsGround;
        bool grounded;



        public Transform orientation;

        float horizontalInput;
        float verticalInput;

        Vector3 moveDirection;

        Rigidbody rb;

        public MovementState state;
        public enum MovementState
        {
            freeze,
            grappling,
            walking,
            air
        }

        public bool freeze;

        public bool activeGrapple;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            readyToJump = true;

            
        }

        private void Update()
        {
            // ground check
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

            MyInput();
            SpeedControl();
            StateHandler();

            // handle drag
            if (grounded && !activeGrapple)
                rb.drag = groundDrag;
            else
                rb.drag = 0;

            
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void MyInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            // when to jump
            if (Input.GetKey(jumpKey) && readyToJump && grounded)
            {
                readyToJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }

        }

        private void StateHandler()
        {
            // Mode - Freeze
            if (freeze)
            {
                state = MovementState.freeze;
                moveSpeed = 0;
                rb.velocity = Vector3.zero;
            }

            // Mode - Grappling
            else if (activeGrapple)
            {
                state = MovementState.grappling;
                moveSpeed = walkSpeed;
            }

            // Mode - Walking
            else if (grounded)
            {
                state = MovementState.walking;
                moveSpeed = walkSpeed;
            }

            // Mode - Air
            else
            {
                state = MovementState.air;
            }
        }

        private void MovePlayer()
        {
            if (activeGrapple) return;

            // calculate movement direction
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;


            // on ground
            if (grounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

            // in air
            else if (!grounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        }

        private void SpeedControl()
        {
            if (activeGrapple) return;


            // limiting speed on ground or in air
            else
            {
                Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                // limit velocity if needed
                if (flatVel.magnitude > moveSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * moveSpeed;
                    rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
                }
            }
        }

        private void Jump()
        {
            // reset y velocity
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        private void ResetJump()
        {
            readyToJump = true;

        }

        private bool enableMovementOnNextTouch;
        public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
        {
            activeGrapple = true;

            velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
            Invoke(nameof(SetVelocity), 0.1f);

            Invoke(nameof(ResetRestrictions), 3f);
        }

        private Vector3 velocityToSet;
        private void SetVelocity()
        {
            enableMovementOnNextTouch = true;
            rb.velocity = velocityToSet;

        
        }

        public void ResetRestrictions()
        {
            activeGrapple = false;
            
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (enableMovementOnNextTouch)
            {
                enableMovementOnNextTouch = false;
                ResetRestrictions();

                GetComponent<Grappling>().StopGrapple();
            }
        }



        public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
        {
            float gravity = Physics.gravity.y;
            float displacementY = endPoint.y - startPoint.y;
            Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
            Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
                + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

            return velocityXZ + velocityY;
        }

    }
