using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class SlimePossess : MonoBehaviour
{
    private Animator kingSlimeFaceAnimator;
    public bool canUsePossessKeybind = true;
    //set true/false when game is paused/unpaused
    private bool canPossess = true;
    private const float RAY_LENGTH = 25f;
    private const float CROSSHAIR_SENS = 0.75f;
    private UIManager UIScript;
    private ThirdPersonCam cameraScript;
    private DiscoverSlimes discoverScript;
    private SlimeAbilities slimeAbility;
    private SlimeMovement slimeMovement;
    private SlimeFollow slimeFollow;
    private SlimeInput inputScript;
    private SoulMovement soulScript;
    private GameObject kingPlayer;
    private Transform possessCrosshair;
    private Vector2 CrosshairScreenPos;
    private Ray ray;
    private RaycastHit hit;

    private void Awake() {
        // Retrieve slime king player
        kingPlayer = GameObject.FindObjectOfType<KingMovement>().gameObject;
        Transform kingSlimeTransform = kingPlayer.transform.Find("King Slime Obj");
        GameObject kingSlimeFace = kingSlimeTransform.Find("KINGFace").gameObject;
        kingSlimeFaceAnimator = kingSlimeFace.GetComponent<Animator>();

        //script references
        cameraScript = Camera.main.gameObject.GetComponent<ThirdPersonCam>();
        inputScript = GetComponent<SlimeInput>();
        discoverScript = kingPlayer.GetComponent<DiscoverSlimes>();
        UIScript = GameObject.FindWithTag("UI Manager").GetComponent<UIManager>();
        possessCrosshair = GameObject.FindWithTag("World Canvas").transform;

        // finds the first instance of script
        slimeAbility = gameObject.GetComponent<SlimeAbilities>();
        slimeMovement = gameObject.GetComponent<SlimeMovement>();
        slimeFollow = gameObject.GetComponent<SlimeFollow>();

        //if this script is NOT on king obj
        if (kingPlayer != gameObject) {
            //disable slime abilities
            if (slimeAbility)
                slimeAbility.enabled = false;
            //disable slime movement
            if (slimeMovement) {
                slimeMovement.enabled = false;
            }
            GetComponent<Rigidbody>().isKinematic = true;
            this.enabled = false;

            soulScript = kingPlayer.transform.GetChild(2).GetComponent<SoulMovement>();
        }
        else {
            soulScript = transform.GetChild(2).GetComponent<SoulMovement>();
        }

        if(soulScript == null)
            Debug.LogError("Make sure the Possess Soul object is 3rd child of King Slime");
    }

    private void Start() {
        //needs to be in start to give GameEvents enough time to get set up
        //subscribe to events
        GameEvents.current.onPauseEvent += PreventPossess;
        GameEvents.current.onResumeEvent += AllowPossess;
    }

    private void Update() {

        // Prevents possession keybind usage and prevents possession when game is paused
        if (!canUsePossessKeybind || !canPossess) return;

        //if pressing return to King input and not the King, possess King
        if(inputScript.GetReturnToKingInput() && gameObject != kingPlayer)
            PosessSlime(kingPlayer);

        //if camera is locked in plcae, shoot raycasts and check for slimes
        if(cameraScript.CamIsLocked())
            RaycastForSlime();

        //if lock cam bind is pressed (1 frame)
        if(inputScript.GetLockCamInput()) {
            //disable movement
            slimeMovement.enabled = false;
            //display possess overlay
            UIScript.SetPossessOverlay(true);

            // Possession face
            if (kingSlimeFaceAnimator.GetBool("Possessing") == false) {
                kingSlimeFaceAnimator.SetBool("Possessing", true);
            }
        }
        //if lock cam bind is released (1 frame)
        else if(inputScript.GetUnlockCamInput()) {
            //enable slime movement
            slimeMovement.enabled = true;
            //reset crosshair position to center of screen
            CrosshairScreenPos = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            //place crosshair under map
            possessCrosshair.position = Vector3.up * (-9999);

            // Puts face back to normal
            kingSlimeFaceAnimator.SetBool("Possessing", false);
            //hide possess overlay
            UIScript.SetPossessOverlay(false);
        }
    }

    public void PosessSlime(GameObject otherSlime) {
        //hide possess overlay
        UIScript.SetPossessOverlay(false);

        //make soul appear and move to possessed slime
        soulScript.MoveSoulToSlime(this.transform, otherSlime.transform);
        
        //switch camera
        cameraScript.SwitchCamera(otherSlime);

        //allow slime to possess others (but not iteslf)
        otherSlime.GetComponent<SlimePossess>().enabled = true;
        //disable this script
        this.enabled = false;
    }

    private void RaycastForSlime() {
        //move crosshair position on 2D screen
        CrosshairScreenPos += inputScript.GetLookInput() * CROSSHAIR_SENS;
        //raycast from screen position to world posiiton
        ray = Camera.main.ScreenPointToRay(CrosshairScreenPos);

        //display prompt for 0.1 sec
        UIScript.DisplayPrompt("Hover over slime to possess!", 0.1f);
        
        //if ray hit something
        if(Physics.Raycast(ray, out hit, RAY_LENGTH)) {

            //place crosshair at point of collision
            possessCrosshair.position = hit.point + (hit.normal * 0.05f);
            //make crosshair rotation perpendicular to ray (make it face the camera)
            possessCrosshair.rotation = Quaternion.LookRotation(-hit.normal, transform.up);

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

                //if object is a super slime and is not discovered, display UI and return
                if(otherObject != kingPlayer && !discoverScript.FindSlimeFollower(otherObject.transform)) {
                    UIScript.DisplayPrompt("Can only possess slime followers!", 0.1f);
                    return;
                }
                //if object is King or a discovered Super Slime, display UI and allow possession
                else
                    UIScript.DisplayPrompt("Left Mouse Button to possess!", 0.1f);
                    
            }

            //if pressing left mouse and it is a different slime, then change possession
            if(inputScript.GetPossessInput() && isSlime) {
                PosessSlime(otherObject);
            }
        }
        else {
            //if ray does NOT hit anything, place crosshair at end of ray length and face camera
            possessCrosshair.position = ray.GetPoint(RAY_LENGTH);
            possessCrosshair.rotation = Quaternion.LookRotation(possessCrosshair.position - Camera.main.transform.position, transform.up);
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

        if (gameObject.CompareTag("King Slime"))
            kingSlimeFaceAnimator.SetBool("Possessing", false);

        //place crosshair under map
        if (possessCrosshair) {
            possessCrosshair.position = Vector3.up * (-9999);
        }
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

        //set crosshair to start in middle of screen
        CrosshairScreenPos = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
    }


    // ---------- Methods for pause and resume events ------------------\\

    private void AllowPossess() {
        canPossess = true;

        //enable slime movement for the current slime being possessed
        //  this is in case you are in possess mode while pressing pause
        if(this.enabled == true)
            slimeMovement.enabled = true;
    }

    private void PreventPossess() {
        canPossess = false;

        //reset crosshair position to center of screen
        CrosshairScreenPos = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        //place crosshair under map
        possessCrosshair.position = Vector3.up * (-9999);
    }

    private void OnDestroy() {
        //unsubscribes from event (avoid null reference when slime dies)
        GameEvents.current.onPauseEvent -= PreventPossess;
        GameEvents.current.onResumeEvent -= AllowPossess;
    }
}
