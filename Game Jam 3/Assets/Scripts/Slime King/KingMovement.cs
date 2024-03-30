using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KingMovement : SlimeMovement
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform kingObj;
    private UIManager UIScript;
    private DiscoverSlimes discoverSlimesScript;
    private Rigidbody rb;
    private RaycastHit hit;
    private Ray ray;
    private float currentHeight;
    private int floorLayer;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float hoverSpeed;
    [SerializeField] private float maxYPos;
    [SerializeField] private float minYPos;
    [SerializeField] private float maxDistToSlime;
    [SerializeField] private float hoverHeight;
    [SerializeField] private float heightBuffer;
    [SerializeField] private float rayInterval;
    private float rayCountdown = 0;
    private Transform trackedSlime = null;
    private float xInput;
    private float zInput;
    private float vertSpeed;
    private Vector3 moveDir;


    private void Awake() {
        //grab rigidbody reference and make sure body doesn't fall over
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        discoverSlimesScript = GetComponent<DiscoverSlimes>();
        UIScript = GameObject.FindWithTag("UI Manager").GetComponent<UIManager>();
        floorLayer = LayerMask.NameToLayer("Floor");
    }

    private void FixedUpdate() {
        ConstrainMovement();

        //set XZ velocity
        rb.velocity = new Vector3(moveDir.x * moveSpeed, vertSpeed, moveDir.z * moveSpeed);
    }

    private void Update() {
        GetInput();
        VerticalMovementCalculations();
    }

    private void VerticalMovementCalculations() {
        rayCountdown -= Time.deltaTime;

        //-----CHECK HEIGHT-----
        //spawn a raycast every ray interval seconds
        if(rayCountdown < 0) {
            rayCountdown = rayInterval;

            //spawn a ray down from current position
            ray = new Ray(transform.position, -Vector3.up);
            Debug.DrawRay(transform.position, Vector3.down * hoverHeight, Color.red);
            
            //check the hit info of the raycast
            if(Physics.Raycast(ray, out hit)) {

                //if an object was hit, make the distance our current height
                if(hit.collider.gameObject.layer == floorLayer)  {
                    currentHeight = hit.distance;
                    //Debug.Log(currentHeight);
                }
            }
        }

        //-----SET VERTICAL SPEED-----
        //if too low to ground, move upwards (cap at max height)
        if(currentHeight < hoverHeight - heightBuffer && (transform.position.y < maxYPos)) {
            vertSpeed = hoverSpeed;
            Debug.Log("Moving Up!");
        }
        //if too high to ground, move downwards (cap at min height)
        else if(currentHeight > hoverHeight + heightBuffer && (transform.position.y > minYPos)) {
            vertSpeed = -hoverSpeed;
            Debug.Log("Moving Down!");
        }
        //if correct height above ground, don't move vertically
        else {
            vertSpeed = 0;
            Debug.Log("Not Moving!");
        }
    }

    private void GetInput() {

        //get X axis input
        xInput = Input.GetAxisRaw("Horizontal");
        //get z axis input
        zInput = Input.GetAxisRaw("Vertical");

    }

    private void ConstrainMovement() {

        //find direction of input based on player orientation (relative to camera)
        moveDir = (orientation.forward * zInput) + (kingObj.up * 0) + (orientation.right * xInput);

        //if tracked slime does NOT exist
        if(trackedSlime == null) {
            //check if there are any other slime followers, if so track it
            if(discoverSlimesScript.GetSlimeFollower() != null)
                trackedSlime = discoverSlimesScript.GetSlimeFollower().transform;
            //if no slime followers, do NOT restrict movement
            else {
                return;
            }
        }

        //if the direction the king is moving in is too far from slimes, don't move
        if(Vector3.Distance((transform.position + (moveDir*3)), trackedSlime.position) > maxDistToSlime) {
            moveDir = Vector3.zero;
            //display prompt for 1 sec
            UIScript.DisplayPrompt("Stay with your slime followers", 1f);
        }
    }

    private void OnDisable() {
        // turns off movement (so the king slime doesn't drift off if already moving)
        rb.velocity = Vector3.zero;
    }

}
