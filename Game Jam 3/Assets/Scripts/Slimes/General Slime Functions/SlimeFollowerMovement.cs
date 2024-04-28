using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SlimeFollowerMovement : SlimeMovement
{
    [Header("Settings")]
    [SerializeField] private Transform orientation;
    [SerializeField] private float groundCheckMaxDist = 0.1f;

    [Header("Movement Settings")]
    public float movementSpeed = 2f;
    [SerializeField] private float groundDrag = 3;
    [SerializeField] private float jumpPower = 5;
    [SerializeField] private float jumpCooldown = 0.2f;
    [Header("Audio Settings")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip moveSound;
    private Rigidbody rb;
    private SlimeInput inputScript;
    private Vector2 moveInput;
    private bool jumpInput;
    private float lastJumpTime = 0f;
    private bool onGround = false;
    private bool isFalling = false;
    private bool isJumping = false;
    private bool isMoving = false;
    private Vector3 moveDir;
    private SoundManager soundManager;
    private AudioSource moveSource;
    private Animator slimeMoveAnimator;

    void Awake()
    {
        //grab rigidbody reference and make sure body doesn't fall over
        rb = GetComponent<Rigidbody>();
        //rb.freezeRotation = true; // disabled as individual constraints
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        inputScript = GetComponent<SlimeInput>();
        slimeMoveAnimator = GetComponentInChildren<Animator>();
    }

    // Used for getting inputs and applying jump
    private void Update() {
        // Gets input for movement calculation
        GetInput();

        // Ground check
        GroundCheck();

        // Applies jump if on ground
        if (jumpInput && onGround && lastJumpTime + jumpCooldown < Time.fixedTime) {
            Jump();
        }

        UpdateAnimatorStateMachine();
    }

    private void FixedUpdate() {
        // Applies x-z movement
        ApplyHorizontalMovement();

        // Applies speed constraints
        ConstrainSpeed();
    }

    private void UpdateAnimatorStateMachine() {
        if (slimeMoveAnimator.GetBool("IsMoving") != isMoving)
            slimeMoveAnimator.SetBool("IsMoving", isMoving);
        if (slimeMoveAnimator.GetBool("IsJumping") != isJumping)
            slimeMoveAnimator.SetBool("IsJumping", isJumping);
        if (slimeMoveAnimator.GetBool("IsFalling") != isFalling)
            slimeMoveAnimator.SetBool("IsFalling", isFalling);
        if (slimeMoveAnimator.GetBool("IsGrounded") != onGround)
            slimeMoveAnimator.SetBool("IsGrounded", onGround);
    }

    private void GroundCheck() {
        onGround = Physics.Raycast(transform.position, Vector3.down, groundCheckMaxDist);

        // allows for application of "friction" on ground
        if (onGround) {
            isJumping = false;
            isFalling = false;
            rb.drag = groundDrag;
        }
        else {
            rb.drag = 0;

            if (rb.velocity.y > 0) { // "jumping"
                isJumping = true;
                isFalling = false;
            } else { // "falling"
                isFalling = true;
                isJumping = false;
            }
        }
    }

    private void ApplyHorizontalMovement() {
        // calculates movement
        moveDir = (orientation.forward * moveInput.y) + (orientation.right * moveInput.x);

        // applies movement
        //rb.velocity = moveDir.normalized * movementSpeed + new Vector3(0, rb.velocity.y, 0);
        rb.AddForce(moveDir.normalized * movementSpeed, ForceMode.Impulse);

        // Move sound effect
        float speed = moveDir.sqrMagnitude;
        if (speed > 0.1 && !moveSource && onGround) {
            isMoving = true;
            moveSource = soundManager.PlaySoundEffectOnObject(moveSound, orientation.gameObject, 0.05f);
        } else if ((speed < 0.1 || !onGround) && moveSource) {
            isMoving = false;
            Destroy(moveSource);
        }
    }

    private void Jump() {
        // Sets last jump variable to current time
        lastJumpTime = Time.fixedTime;

        // Resets y-velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Applies jump force
        rb.AddForce(orientation.up * jumpPower, ForceMode.Impulse);

        // Plays jump sound
        soundManager.PlaySoundEffectAtPoint(jumpSound, transform.position, 0.5f);
    }

    private void ConstrainSpeed() {
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit velocity if faster than movement speed
        if (horizontalVelocity.magnitude > movementSpeed) {
            Vector3 limitedVel = horizontalVelocity.normalized * movementSpeed;

            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void GetInput() {
        //get input from input map
        moveInput = inputScript.GetMoveInput();
        jumpInput = inputScript.GetJumpInput();
    }
}
