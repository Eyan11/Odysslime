using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Choose How the Activation Plate is Activated: ")]
    [SerializeField] private PlateType plateType;

    [Header("References, Leave Empty of Not Applicable")]
    [SerializeField] private GateOpen gateScript;
    [SerializeField] private BridgeExtend bridgeScript;
    [SerializeField] private Material activatedMaterial;

    [Header("Slime Plate Settings: ")]
    [SerializeField] private int slimesToActivate;
    private int slimesOnPlate = 0;
    private int iceCubesOnPlate = 0;
    private bool plateIsActive = false;
    private int slimeLayer, iceLayer, colliderLayer;
    private GameObject imageDisplay;
    private GameObject promptDisplay;
    private TextMeshProUGUI textBox;

    private void UpdateDisplayText() {
        // Displays prompt only if plate is not active
        if (plateIsActive) {
            promptDisplay.SetActive(false);
            return;
        } else if (!promptDisplay.activeSelf) {
            promptDisplay.SetActive(true);
        }

        if (plateType == PlateType.Slime) { // slime UI
            int slimesNeeded = slimesToActivate - slimesOnPlate;
            
            textBox.text = "" + (slimesNeeded);
        } else if (plateType == PlateType.IceCube) { // ice cube UI
            textBox.text = ""; // Blank
        }
    }

    private void Awake() {
        slimeLayer = LayerMask.NameToLayer("Slime");
        iceLayer = LayerMask.NameToLayer("Ice");

        // GUI
        promptDisplay = transform.Find("GUI").gameObject;
        // textbox
        textBox = promptDisplay.transform.Find("AmountText").GetComponent<TextMeshProUGUI>();
        // image & description
        if (plateType == PlateType.Slime) { // slime UI
            imageDisplay = promptDisplay.transform.Find("SlimeImage").gameObject;
        } else if (plateType == PlateType.IceCube) { // ice cube UI
            imageDisplay = promptDisplay.transform.Find("IceCubeImage").gameObject;
        }
        UpdateDisplayText();
        imageDisplay.SetActive(true);
    }

    private void OnTriggerEnter(Collider other) {
        colliderLayer = other.gameObject.layer;

        //if plate is activated by slimes and slime entered the plate
        if((plateType == PlateType.Slime) && (colliderLayer == slimeLayer)) {
            //add slime to plate
            UpdateSlimePlate(1);
        }
        //if plate is activated by ice cubes and ice cube entered the plate
        else if((plateType == PlateType.IceCube) && (colliderLayer == iceLayer)) {
            //add ice cube to plate
            UpdateIcePlate(1);
        }
    }

    private void OnTriggerExit(Collider other) {
        colliderLayer = other.gameObject.layer;

        //if plate is activated by slimes and slime exited the plate
        if((plateType == PlateType.Slime) && (colliderLayer == slimeLayer)) {
            //remove slime to plate
            UpdateSlimePlate(-1);
        }
        //if plate is activated by ice cubes and ice cube exited the plate
        else if((plateType == PlateType.IceCube) && (colliderLayer == iceLayer)) {
            //remove ice cube to plate
            UpdateIcePlate(-1);
        }
    }

    private void UpdateSlimePlate(int slimeChange) {
        slimesOnPlate += slimeChange;
        UpdateDisplayText();

        //if plate is NOT activated and enough slimes are on plate
        if(!plateIsActive && slimesOnPlate >= slimesToActivate) {
            //activate plate
            plateIsActive = true;
            
            //activate gate or bridge
            if(gateScript)
                gateScript.Activate();
            else if(bridgeScript)
                bridgeScript.Activate();

            //switch materials once activated
            GetComponent<Renderer>().material = activatedMaterial;
        }
    }

    private void UpdateIcePlate(int iceChange) {
        iceCubesOnPlate += iceChange;
        UpdateDisplayText();

        //if plate is NOT activated and enough ice cubes are on plate
        if(!plateIsActive && iceCubesOnPlate >= 1) {
            //activate plate
            plateIsActive = true;

            //activate gate or bridge
            if(gateScript)
                gateScript.Activate();
            else if(bridgeScript)
                bridgeScript.Activate();

            //switch materials once activated
            GetComponent<Renderer>().material = activatedMaterial;
        }
    }

    public bool IsPlateActive() {
        return plateIsActive;
    }



/*   FOR PRESSURE PLATE - KEEPING INCASE WE WANT IT BACK

    private void UpdateSlimePlate(int slimeChange) {
        slimesOnPlate += slimeChange;

        //if plate is NOT activated and enough slimes are on plate
        if(!plateIsActive && slimesOnPlate >= slimesToActivate) {
            //activate plate
            plateIsActive = true;
            
            if(gateScript)
                gateScript.Activate();
            else if(bridgeScript)
                bridgeScript.Activate();
        }

        //if plate IS activated and NOT enough slimes are on plate
        else if(plateIsActive && slimesOnPlate < slimesToActivate) {
            //DEactivate plate
            plateIsActive = false;

            if(gateScript)
                gateScript.Deactivate();
            else if(bridgeScript)
                bridgeScript.Deactivate();
        }
    }

    //right now ice plate only requires one ice cube to activate
    //since I can't check if ice cubes are stacked on top of each other without more work
    private void UpdateIcePlate(int iceChange) {
        iceCubesOnPlate += iceChange;

        //if plate is NOT activated and enough ice cubes are on plate
        if(!plateIsActive && iceCubesOnPlate >= 1) {
            //activate plate
            plateIsActive = true;
            Debug.Log("Plate is activated");

            if(gateScript)
                gateScript.Activate();
            else if(bridgeScript)
                bridgeScript.Activate();
        }

        //if plate IS activated and NOT enough ice cubes are on plate
        else if(plateIsActive && iceCubesOnPlate < 1) {
            //deactivate plate
            plateIsActive = false;
            Debug.Log("Plate is deactivated");

            if(gateScript)
                gateScript.Deactivate();
            else if(bridgeScript)
                bridgeScript.Deactivate();
        }
    }
*/

    //enums are a set of named values
    //PlateType specifies what activates this pressure plate
    private enum PlateType {
        Slime, IceCube
    }

}
