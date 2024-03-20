using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SlimeFollowerMovement : SlimeMovement
{
    [Header("Settings")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform slimeObj;
    [SerializeField] private float bodyRadius = 0.5f;
    [Header("Movement Settings")]
    public float movementSpeed = 2f;
    [SerializeField] private float groundDrag = 3;
    [SerializeField] private float jumpPower = 5;
    [SerializeField] private float jumpCooldown = 0.2f;
    
    private Rigidbody rb;
    private float xInput;
    private float zInput;
    private float yInput;
    private float lastJumpTime = 0f;
    private bool onGround = false;
    private float jumpStart;
    private Vector3 moveDir;
    private RaycastHit raycastHit;
    private SoundManager soundManager;

    void Awake()
    {
        //grab rigidbody reference and make sure body doesn't fall over
        rb = GetComponent<Rigidbody>();
        //rb.freezeRotation = true; // disabled as individual constraints
        soundManager = GameObject.FindObjectOfType<SoundManager>();
    }

    private void Update() {
        // Ground check
        GroundCheck();

        // Gets input for movement calculation
        GetInput();

        // Applies x-z movement
        ApplyHorizontalMovement();

        // Applies jump if on ground
        if (yInput == 1 && lastJumpTime + jumpCooldown < Time.fixedTime) {
            Jump();
        }

        // Applies speed constraints
        ConstrainSpeed();
    }

    private void GroundCheck() {
        onGround = Physics.Raycast(transform.position, Vector3.down, bodyRadius);

        // allows for application of "friction" on ground
        if (onGround) {
            rb.drag = groundDrag;
        }
        else {
            rb.drag = 0;
        }
    }

    private void ApplyHorizontalMovement() {
        // calculates movement
        moveDir = (orientation.forward * zInput) + (orientation.right * xInput);

        // applies movement
        rb.AddForce(moveDir.normalized * movementSpeed, ForceMode.Impulse);
    }

    private void Jump() {
        // Sets last jump variable to current time
        lastJumpTime = Time.fixedTime;

        // Resets y-velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Applies jump force
        rb.AddForce(orientation.up * jumpPower, ForceMode.Impulse);

        // Plays jump sound
        soundManager.PlaySlimeJump();
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
        //get X axis input
        xInput = Input.GetAxisRaw("Horizontal");
        //get z axis input
        zInput = Input.GetAxisRaw("Vertical");

        //get y axis input (space is up, shift is down)
        if(Input.GetKey(KeyCode.Space) && onGround)
            yInput = 1;
        else
            yInput = 0;
    }
}
