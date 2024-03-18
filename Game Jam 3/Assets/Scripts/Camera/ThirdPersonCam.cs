using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    //these is for the object you start the game as (slime king)
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform player;
    [SerializeField] private Transform obj;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private GameObject thirdPersonCam;
    [SerializeField] private CinemachineFreeLook thirdPersonFreeLookCam;
    [SerializeField] private GameObject topDownCam;
    [SerializeField] private CinemachineFreeLook topDownFreeLookCam;

    [Header("Settings")]
    [SerializeField] private float rotationSpeed;

    //Time cam is unlocked while transitioning to another slime
    [SerializeField] private float camUnlockedTime;
    [SerializeField] private float mouseSensX = 300f;
    [SerializeField] private float mouseSensY = 2f;
    private float camUnlockedTimer = 0f;
    private bool camIsLocked = false;

    private void Awake() {
        //makes mouse invisible and locked in place
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void Update() {
        MoveOrientation();
        CameraLockState();
    }

    private void MoveOrientation() {
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
            obj.forward = Vector3.Slerp(obj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
    }

    //determines if camera should be locked in place or not
    private void CameraLockState() {
        camUnlockedTimer -= Time.deltaTime;

        //lock cam on first frame right mouse is pressed
        if(Input.GetMouseButtonDown(1)) {
            LockCamera();
        }
        //unlock cam on first frame right mouse is released
        else if(Input.GetMouseButtonUp(1))
            UnlockCamera();
    }

    
    private void LockCamera() {
        //don't lock camera if still transitioning to another slime
        if(camUnlockedTimer > 0)
            return;

        //lock cursor to game window
        Cursor.lockState = CursorLockMode.Confined;
        //make cursor visible
        Cursor.visible = true;

        //turn camera sensitivity to 0
        thirdPersonFreeLookCam.m_XAxis.m_MaxSpeed = 0f;
        thirdPersonFreeLookCam.m_YAxis.m_MaxSpeed = 0f;
        topDownFreeLookCam.m_XAxis.m_MaxSpeed = 0f;
        topDownFreeLookCam.m_YAxis.m_MaxSpeed = 0f;
        camIsLocked = true;
    }

    private void UnlockCamera() {

        //lock cursor to center of game view
        Cursor.lockState = CursorLockMode.Locked;
        //make cursor invisible
        Cursor.visible = false;

        //return camera sensitivity to original values
        thirdPersonFreeLookCam.m_XAxis.m_MaxSpeed = mouseSensX;
        thirdPersonFreeLookCam.m_YAxis.m_MaxSpeed = mouseSensY;
        topDownFreeLookCam.m_XAxis.m_MaxSpeed = mouseSensX;
        topDownFreeLookCam.m_YAxis.m_MaxSpeed = mouseSensY;
        camIsLocked = false;
    }

    //returns true if camera is locked (freelook component is disabled)
    public bool CamIsLocked() {
        return camIsLocked;
    }


    //Called when camera needs to be switched because of possess ability
    public void SwitchCamera(GameObject slimePlayer) {

        //force camera to be unlocked for short time
        camUnlockedTimer = camUnlockedTime;
        UnlockCamera();

        //disable all camera's
        thirdPersonCam.SetActive(false);
        topDownCam.SetActive(false);

        //switch to king camera settings
        if(slimePlayer.CompareTag("Slime King")) {
            //enable top down cam
            topDownCam.SetActive(true);
        }
        //switch to slime camera settings
        else {
            //enable third person cam
            thirdPersonCam.SetActive(true);

            //change settings to follow designated slime
            thirdPersonFreeLookCam.m_LookAt = slimePlayer.transform;
            thirdPersonFreeLookCam.m_Follow = slimePlayer.transform;
        }

        //get references for player rotation when moving
        player = slimePlayer.transform;
        rb = slimePlayer.GetComponent<Rigidbody>();

        //get obj, should be child(0), but checking both just in case someone moves it
        if(slimePlayer.gameObject.transform.GetChild(0).CompareTag("Obj"))
            obj = slimePlayer.gameObject.transform.GetChild(0);
        else if(slimePlayer.gameObject.transform.GetChild(1).CompareTag("Obj"))
            obj = slimePlayer.gameObject.transform.GetChild(1);
        else
            Debug.Log("Can't get obj reference. Make sure Slime obj is first child of slime player!");
        
        //get orientation, should be child(1), but checking both just in case someone moves it
        if(slimePlayer.gameObject.transform.GetChild(1).CompareTag("Orientation"))
            orientation = slimePlayer.gameObject.transform.GetChild(1);
        else if(slimePlayer.gameObject.transform.GetChild(0).CompareTag("Orientation"))
            orientation = slimePlayer.gameObject.transform.GetChild(0);
        else
            Debug.Log("Can't get orientation reference. Make sure Slime obj is second child of slime player!");
    }


}
