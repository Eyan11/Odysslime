using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePossess : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ThirdPersonCam cameraScript;
    [SerializeField] private GameObject slimeKingPlayer;
    private Ray ray;
    private RaycastHit hit;

    [Header("Scripts to Disable/Enable")]
    [SerializeField] private KingPossess kingPossessScript;

    private void Awake() {
        //disable this script on start
        this.enabled = false;
    }

    private void Update() {

        //press Z to return to king slime
        if(Input.GetKeyDown(KeyCode.Z)) {
            //Testing
            Debug.Log("Z input, possessing king!");

            PossessKing();
        }

        //press E to possess another slime
        if(Input.GetKeyDown(KeyCode.E))
            TryToPossessSlime();

    }

    private void PossessKing() {

        //change camera and disable script
        cameraScript.SwitchCamera(slimeKingPlayer);

        //possess king, enable king script
        kingPossessScript.enabled = true;
        //stop possessing this slime, disable this script
        this.enabled = false;
    }

    private void TryToPossessSlime() {
        //spawn ray from screen to cursor position in world
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        //if ray hit something
        if(Physics.Raycast(ray, out hit)) {

            //Testing
            Debug.Log("Mouse raycast hit: " + hit.collider.name);

            //if ray hit a slime follower, possess slime
            if(hit.collider.gameObject.CompareTag("Slime Follower")) {

                //Testing
                Debug.Log("Possessing slime follower!");

                //switch camera
                cameraScript.SwitchCamera(hit.collider.gameObject);

                //allow slime to possess others
                hit.collider.gameObject.GetComponent<SlimePossess>().enabled = true;
                //disable this script
                this.enabled = false;
            }

            //if ray cast hit slime king, possess king
            else if(hit.collider.gameObject.CompareTag("Slime King")) {
                //Testing
                Debug.Log("Possessing slime king!");
                
                PossessKing();
            }

        }
    }



    //run every time this script is disabled in inspector
    private void OnDisable() {
        //disable other slime scripts no longer in use
        //for Von
    }

    //run every time this script is enabled in inspector
    private void OnEnable() {
        //enable other slime scripts now in use
        //for Von
    }

}
