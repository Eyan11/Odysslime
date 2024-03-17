using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingPossess : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ThirdPersonCam cameraScript;
    private Ray ray;
    private RaycastHit hit;

    [Header("Scripts to Disable/Enable")]
    [SerializeField] private KingMovement kingMoveScript;


    //only works when mouse is visible rn, press esc to make cursor visible again
    private void Update() {
        
        //if input "E", try to possess a slime
        if(Input.GetKeyDown(KeyCode.E)) {
            //Only possess a slime when raycast hits a slime
            TryToPossessSlime();
        }
    }

    private void TryToPossessSlime() {
        //spawn ray from screen to cursor position in world
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        //if ray hit something
        if(Physics.Raycast(ray, out hit)) {

            //Testing
            Debug.Log("Mouse raycast hit: " + hit.collider.name);

            //if ray hit a slime follower
            if(hit.collider.gameObject.CompareTag("Slime Follower")) {

                //Testing
                Debug.Log("Possessing slime follower from " + gameObject.name + "!");

                //switch camera
                cameraScript.SwitchCamera(hit.collider.gameObject);

                //allow slime to possess others (but not itself)
                hit.collider.gameObject.GetComponent<SlimePossess>().enabled = true;
                hit.collider.gameObject.layer = 2;
                //disable this script
                this.enabled = false;
            }
        }
    }

    //run every time this script is disabled in inspector
    private void OnDisable() {
        //disable other king slime scripts no longer in use
        kingMoveScript.enabled = false;
    }

    //run every time this script is enabled in inspector
    private void OnEnable() {
        //enable other king slime scripts now in use
        kingMoveScript.enabled = true;
    }

}
