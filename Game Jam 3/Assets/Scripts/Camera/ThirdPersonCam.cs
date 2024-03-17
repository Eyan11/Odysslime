using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerObj;
    [SerializeField] private Rigidbody rb;

    [Header("Settings")]
    [SerializeField] private float rotationSpeed;

    private void Start() {
        //makes mouse invisible and locked in place
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {

        //calculates the direction the camera is facing relative to player (vector on XZ plane from camera to player)
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        //sets the forward direction to the vector calculated (forward is the direction camera is looking)
        orientation.forward = viewDir.normalized;

        //get horizontal and vertical keyboard inputs
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //make the input relative to the direction the camera is facing
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //smoothly change (Slerp) player orientation to match the input direction
        if(inputDir != Vector3.zero)
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);

    }


}
