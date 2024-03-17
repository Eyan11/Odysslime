using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePossess : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ThirdPersonCam cameraScript;
    [SerializeField] private GameObject slimeKingPlayer;
    private SlimeAbilities slimeAbility;
    private SlimeMovement slimeMovement;
    private Ray ray;
    private RaycastHit hit;

    private void Awake() {
        // finds the first instance of a slimeAbility component
        slimeAbility = gameObject.GetComponent<SlimeAbilities>();

        // finds the first instance of a slimeMovement component
        slimeMovement = gameObject.GetComponent<SlimeMovement>();

        //disable this script on start if not the slime king
        if (slimeKingPlayer != gameObject) {
            if (slimeAbility) {
                slimeAbility.enabled = false;
            }
            if (slimeMovement) {
                slimeMovement.enabled = false;
            }
            this.enabled = false;
        }
    }

    private void Update() {

        //if press "Z" and not the king, return to king slime
        if(Input.GetKeyDown(KeyCode.Z) && gameObject != slimeKingPlayer) {
            //Testing
            Debug.Log("Z input, possessing king!");

            PosessSlime(slimeKingPlayer);
        }


        //if right mouse is held (camera is locked)
        if(cameraScript.CamIsLocked()) {
            Debug.Log("Camera is LOCKED!");
            //check for a slime to possess
            RaycastForSlime();
            slimeMovement.enabled = false;
        }
        else {
            Debug.Log("Camera is UNLOCKED!");
            slimeMovement.enabled = true;
        }

    }

    private void PosessSlime(GameObject otherSlime) {
        //Testing
        Debug.Log("Possessing " + otherSlime.name  + " from " + gameObject.name + "!");

        //switch camera
        cameraScript.SwitchCamera(otherSlime);

        //allow slime to possess others (but not ieslf)
        otherSlime.GetComponent<SlimePossess>().enabled = true;
        //disable this script
        this.enabled = false;
    }

    private void RaycastForSlime() {
        //spawn ray from screen to cursor position in world
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        //if ray hit something
        if(Physics.Raycast(ray, out hit)) {

            //Testing
            //Debug.Log("Mouse raycast hit: " + hit.collider.name);

            //store hit object in otherObject
            GameObject otherObject = hit.collider.gameObject;
            bool isSlime = false;

            //if ray cast hit any slime
            if (otherObject.CompareTag("Slime Follower") || otherObject.CompareTag("Slime King")) {
                isSlime = true;

                //INSERT METHOD CALL FOR SWITCHING SLIMES UI
            }

            //if pressing "E" and it is a different slime, then change possession
            if(Input.GetKeyDown(KeyCode.E) && isSlime && otherObject != gameObject) {
                PosessSlime(otherObject);
            }
        }
    }



    //run every time this script is disabled in inspector
    private void OnDisable() {
        //reallows raycast to hit slime
        gameObject.layer = 0;

        //disable slime's ability if one exists
        if (slimeAbility) {
            slimeAbility.enabled = false;
        }
        //disable slime's movement if it exists
        if (slimeMovement) {
            slimeMovement.enabled = false;
        }
    }

    //run every time this script is enabled in inspector
    private void OnEnable() {
        //prevents raycast from hitting oneself
        gameObject.layer = 2;
        //enable slime's ability if one exists
        if (slimeAbility) {
            slimeAbility.enabled = true;
        }
        //enable slime's movement if it exists
        if (slimeMovement) {
            slimeMovement.enabled = true;
        }
    }

}
