using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    //these references are for the object you start the game as (King Slime)
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform player;
    [SerializeField] private Transform obj;

    [SerializeField] private GameObject thirdPersonCam;
    [SerializeField] private CinemachineFreeLook thirdPersonFreeLookCam;
    [SerializeField] private GameObject topDownCam;
    [SerializeField] private CinemachineFreeLook topDownFreeLookCam;
    private SlimeInput inputScript;
    private Vector2 moveInput;

    [Header("Settings")]
    [SerializeField] private float orientationRotSpeed;
    [SerializeField] private float mouseSensX = 0.1f;
    [SerializeField] private float mouseSensY = 0.0007f;
    private bool camIsLocked = false;

    private void Awake() {
        //makes mouse invisible and locked in place
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputScript = GetComponent<SlimeInput>();
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

        //get movement input from input map
        moveInput = inputScript.GetMoveInput();
        
        //make the input relative to the direction the camera is facing
        Vector3 inputDir = orientation.forward * moveInput.y + orientation.right * moveInput.x;

        //smoothly change (Slerp) player orientation to match the input direction
        if(obj && inputDir != Vector3.zero)
            obj.forward = Vector3.Slerp(obj.forward, inputDir.normalized, Time.deltaTime * orientationRotSpeed);
    }

    //determines if camera should be locked in place or not
    private void CameraLockState() {

        //lock cam if unlocked and holding lock input
        if(!camIsLocked && inputScript.GetLockCamInput()) {
            LockCamera();
        }
        //unlock cam if locked and NOT holding lock input
        else if(camIsLocked && inputScript.GetUnlockCamInput())
            UnlockCamera();
    }

    
    public void LockCamera() {
        camIsLocked = true;

        //set camera sensitivity to 0
        SetCamSensitivity(0f);
    }

    public void UnlockCamera() {
        camIsLocked = false;

        //return camera sensitivity to original values
        SetCamSensitivity(1f);
    }

    public void SetCamSensitivity(float sensitivity) {

        //set sensitivity of camera's
        thirdPersonFreeLookCam.m_XAxis.m_MaxSpeed = mouseSensX * sensitivity;
        thirdPersonFreeLookCam.m_YAxis.m_MaxSpeed = mouseSensY * sensitivity;
        topDownFreeLookCam.m_XAxis.m_MaxSpeed = mouseSensX * sensitivity;
        topDownFreeLookCam.m_YAxis.m_MaxSpeed = mouseSensY * sensitivity;
    }

    //returns true if camera is locked
    public bool CamIsLocked() {
        return camIsLocked;
    }


    //Called when camera needs to be switched because of possess ability
    public void SwitchCamera(GameObject slimePlayer) {
        //force camera to be unlocked for short time
        UnlockCamera();

        //disable all camera's
        thirdPersonCam.SetActive(false);
        topDownCam.SetActive(false);

        //switch to king camera settings
        if(slimePlayer.CompareTag("King Slime")) {
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


        //-----Getting reference to orientation and obj children-----\\

        // For slimes
        if (slimePlayer.layer == 11) { // Slime player
            // resets third person free cam back to normal orbits
            thirdPersonFreeLookCam.m_Orbits[0].m_Height = 6; // top
            thirdPersonFreeLookCam.m_Orbits[0].m_Radius = 3;
            thirdPersonFreeLookCam.m_Orbits[1].m_Height = 3; // middle
            thirdPersonFreeLookCam.m_Orbits[1].m_Radius = 6;
            thirdPersonFreeLookCam.m_Orbits[2].m_Height = 0; // bottom
            thirdPersonFreeLookCam.m_Orbits[2].m_Radius = 7;

            //get obj, should be child(0), but checking both just in case someone moves it
            if(slimePlayer.transform.GetChild(0).CompareTag("Obj"))
                obj = slimePlayer.transform.GetChild(0);
            else if(slimePlayer.transform.GetChild(1).CompareTag("Obj"))
                obj = slimePlayer.transform.GetChild(1);
            else
                Debug.LogError("Make sure Slime obj is first child of slime player!");
            
            //get orientation, should be child(1), but checking both just in case someone moves it
            if(slimePlayer.transform.GetChild(1).CompareTag("Orientation"))
                orientation = slimePlayer.transform.GetChild(1);
            else if(slimePlayer.transform.GetChild(0).CompareTag("Orientation"))
                orientation = slimePlayer.transform.GetChild(0);
            else
                Debug.LogError("Make sure Slime orientation is second child of slime player!");
        }
        else if (slimePlayer.GetComponent<Movable>()) {
            // calculates new orbits based on size
            Vector3 objSize = slimePlayer.transform.localScale;
            float maxSize = Mathf.Max(objSize.x, objSize.y, objSize.z);
            float scale = Mathf.Max(maxSize / 5.0f, 1);

            thirdPersonFreeLookCam.m_Orbits[0].m_Height = 6 * scale; // top
            thirdPersonFreeLookCam.m_Orbits[0].m_Radius = 3 * scale;
            thirdPersonFreeLookCam.m_Orbits[1].m_Height = 3 * scale; // middle
            thirdPersonFreeLookCam.m_Orbits[1].m_Radius = 6 * scale;
            thirdPersonFreeLookCam.m_Orbits[2].m_Height = 0 * scale; // bottom
            thirdPersonFreeLookCam.m_Orbits[2].m_Radius = 7 * scale;

            Transform movableOrientation = slimePlayer.transform.Find("Orientation");

            // Creates an orientation game object based on the existing movable
            if (!movableOrientation) {
                movableOrientation = new GameObject("Orientation").transform;
                movableOrientation.position = slimePlayer.transform.position;
                movableOrientation.SetParent(slimePlayer.transform);
            }

            orientation = movableOrientation;
            obj = null;
        }
        else {
            Debug.LogError("Unexpected object " + slimePlayer.name + " is attempting to use the SwitchCamera method!");
        }
    }

}
