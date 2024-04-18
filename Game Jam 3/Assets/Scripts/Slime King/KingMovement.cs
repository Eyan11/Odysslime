using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KingMovement : SlimeMovement
{
    private Transform kingObj;
    private Transform orientation;
    private UIManager UIScript;
    private SlimeInput inputScript;
    private Rigidbody rb;
    private RaycastHit hit;
    private Ray ray;
    private int floorLayer;

    [Header("Horizontal Movement Settings")]
    [SerializeField] private float moveSpeed;
    private Vector2 moveInput;
    private Vector3 moveDir;
    private Vector3 groundPos;
    
    [Header ("Vertical Movement Settings")]
    [SerializeField] private float maxHoverSpeed;
    [SerializeField] private float hoverAcceleration;
    [SerializeField] private float hoverDeacceleration;
    [SerializeField] private float maxYPos;
    [SerializeField] private float minYPos;
    [SerializeField] private float hoverHeight;
    [SerializeField] private float heightBuffer;
    private float vertSpeed;

    [Header ("Downwards Raycast Settings")]
    [SerializeField] private float rayInterval;
    private float rayCountdown = 0;
    private float currentHeight;

    private void Awake() {
        kingObj = transform.GetChild(0);
        orientation = transform.GetChild(1);

        //send error message if accidentally switched order or children
        if(kingObj.CompareTag("Obj") == false || orientation.CompareTag("Orientation") == false)
            Debug.LogError("King Slime Obj needs to be first child and Orientation needs to be second child of King Slime Player");

        //make sure rigidbody doesn't fall over
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        inputScript = GetComponent<SlimeInput>();
        UIScript = GameObject.FindWithTag("UI Manager").GetComponent<UIManager>();
        floorLayer = LayerMask.NameToLayer("Floor");
    }

    private void FixedUpdate() {
        //set XZ velocity
        rb.velocity = new Vector3(moveDir.x * moveSpeed, vertSpeed, moveDir.z * moveSpeed);
        Debug.Log("Vertical Speed: " + vertSpeed);
    }

    private void Update() {
        rayCountdown -= Time.deltaTime;

        //every rayInterval seconds:
        if(rayCountdown < 0) {
            rayCountdown = rayInterval;
            
            //find height above the ground
            groundPos = RayCastDownPosition();
            currentHeight = transform.position.y - groundPos.y;
            //Debug.Log("currentHeight: " + currentHeight);

            //set the vertical speed based on current height
            SetVerticalSpeed();
        }

        GetInput();
    }

    //returns the point fo collision on a downwards raycast
    public Vector3 RayCastDownPosition() {

        //spawn a ray down from current position
        ray = new Ray(transform.position, -Vector3.up);
        
        //check the hit info of the raycast
        if(Physics.Raycast(ray, out hit)) {

            //if an object with "Floor" layer was hit, return it's position
            if(hit.collider.gameObject.layer == floorLayer)
                return hit.point;
        }

        //if floor wasn't hit, return where the floor would be based on currentHeight
        return transform.position - Vector3.up * currentHeight;
    }

    private void SetVerticalSpeed() {

        //if too low to floor, MOVE DOWN
        if(currentHeight < (hoverHeight - heightBuffer))
            vertSpeed += hoverAcceleration * Time.deltaTime;
        //if too high to floor, MOVE UP
        else if(currentHeight > (hoverHeight + heightBuffer))
            vertSpeed -= hoverAcceleration * Time.deltaTime;
        //if within buffers and speed is NOT close to zero, deaccelerate towards 0
        else if(Mathf.Abs(vertSpeed) > 0.5)
            vertSpeed += -Mathf.Sign(vertSpeed) * hoverDeacceleration * Time.deltaTime;
        //if within buffers and speed IS close to zero, set speed to exactly 0
        else
            vertSpeed = 0f;

        //if moving up and above max height, STOP MOVING
        if(vertSpeed > 0 && transform.position.y > maxYPos)
            vertSpeed = 0f;
        //if moving down and below min height, STOP MOVING
        else if(vertSpeed < 0 && transform.position.y < minYPos)
            vertSpeed = 0f;

        //Clamp speed between negative max speed and positive max speed
        vertSpeed = Mathf.Clamp(vertSpeed, -maxHoverSpeed, maxHoverSpeed);
    }

    private void GetInput() {
        //get input from input map
        moveInput = inputScript.GetMoveInput();

        //find direction of input based on player orientation (relative to camera)
        moveDir = (orientation.forward * moveInput.y) + (kingObj.up * 0) + (orientation.right * moveInput.x);
    }

    private void OnEnable() {
        // turns on movement
        rb.isKinematic = false;
    }

    private void OnDisable() {
        // turns off movement (so the king slime doesn't drift off if already moving)
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }
}
