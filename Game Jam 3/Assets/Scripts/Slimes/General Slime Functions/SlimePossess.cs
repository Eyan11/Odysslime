using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class SlimePossess : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Texture2D scopeImage;
    private UIManager UIScript;
    private ThirdPersonCam cameraScript;
    private GameObject slimeKingPlayer;
    private DiscoverSlimes discoverScript;
    private SlimeAbilities slimeAbility;
    private SlimeMovement slimeMovement;
    private SlimeFollow slimeFollow;
    private Ray ray;
    private RaycastHit hit;
    private SlimeInput inputScript;

    private void Awake() {
        // Retrieve slime king player
        slimeKingPlayer = GameObject.FindObjectOfType<KingMovement>().gameObject;

        //script references
        cameraScript = Camera.main.gameObject.GetComponent<ThirdPersonCam>();
        inputScript = GetComponent<SlimeInput>();
        discoverScript = slimeKingPlayer.GetComponent<DiscoverSlimes>();
        UIScript = GameObject.FindWithTag("UI Manager").GetComponent<UIManager>();

        // finds the first instance of script
        slimeAbility = gameObject.GetComponent<SlimeAbilities>();
        slimeMovement = gameObject.GetComponent<SlimeMovement>();
        slimeFollow = gameObject.GetComponent<SlimeFollow>();

        //if this script is NOT on king obj
        if (slimeKingPlayer != gameObject) {
            //disable slime abilities
            if (slimeAbility)
                slimeAbility.enabled = false;
            //disable slime movement
            if (slimeMovement) {
                slimeMovement.enabled = false;
            }
            GetComponent<Rigidbody>().isKinematic = true;
            this.enabled = false;
        }
    }

    private void FixedUpdate() {

        //if pressing return to King input and not the king
        if(inputScript.GetReturnToKingInput() && gameObject != slimeKingPlayer) {
            //return to king Slime
            PosessSlime(slimeKingPlayer);
        }


        //if camera is locked in plcae
        if(cameraScript.CamIsLocked()) {
            // changes mouse icon to scope
            //Cursor.SetCursor(scopeImage, Vector2.zero, CursorMode.Auto);

            //check for a slime to possess
            RaycastForSlime();
            slimeMovement.enabled = false;
        }
        else {
            // changes mouse icon back to normal
            //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            slimeMovement.enabled = true;
        }

    }

    public void PosessSlime(GameObject otherSlime) {
        //if trying to possess current slime, don't allow it
        if (otherSlime == gameObject)
            return;

        //if otherSlime is not king and not currently a slime follower, do NOT possess them
        if(!otherSlime.CompareTag("King Slime") && discoverScript.FindSlimeFollower(otherSlime.transform) == -1)
            return;

        //switch camera
        cameraScript.SwitchCamera(otherSlime);

        //allow slime to possess others (but not iteslf)
        otherSlime.GetComponent<SlimePossess>().enabled = true;
        //disable this script
        this.enabled = false;
    }

    private void RaycastForSlime() {
        //spawn ray from screen to cursor position in world
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 20, Color.red);

        //display prompt for 0.1 sec
        UIScript.DisplayPrompt("Hover over slime to possess!", 0.1f);
        
        //if ray hit something
        if(Physics.Raycast(ray, out hit)) {
            //Testing
            //Debug.Log("Mouse raycast hit: " + hit.collider.name);

            //store hit object in otherObject
            GameObject otherObject = hit.collider.gameObject;
            bool isSlime = false;

            // throw out results if self
            if (otherObject == gameObject) {
                return;
            }

            //if ray cast hit any slime (except oneself)
            if (otherObject.CompareTag("Super Slime") || otherObject.CompareTag("King Slime")) {
                isSlime = true;

                //display prompt for 0.1 sec
                UIScript.DisplayPrompt("Left Mouse Button to possess!", 0.1f);
            }

            //if pressing left mouse and it is a different slime, then change possession
            if(inputScript.GetPossessInput() && isSlime) {
                PosessSlime(otherObject);
            }
        }
    }



    //run every time this script is disabled in inspector
    private void OnDisable() {
        //disable scripts if they exists
        if (slimeAbility)
            slimeAbility.enabled = false;
        
        if (slimeMovement)
            slimeMovement.enabled = false;

        //make slime "undiscoverd" after possessing
        if (slimeFollow) {
            slimeFollow.enabled = false;
            discoverScript.RemoveSlimeFollower(this.transform);
        }

        if(gameObject.CompareTag("Super Slime"))
            GetComponent<Rigidbody>().isKinematic = true;
    }

    //run every time this script is enabled in inspector
    private void OnEnable() {
        //enable scripts if they exists
        if (slimeAbility)
            slimeAbility.enabled = true;

        if (slimeMovement)
            slimeMovement.enabled = true;
        
        if (slimeFollow)
            slimeFollow.enabled = false;   

        if(gameObject.CompareTag("Super Slime"))
            GetComponent<Rigidbody>().isKinematic = false; 
    }

}
