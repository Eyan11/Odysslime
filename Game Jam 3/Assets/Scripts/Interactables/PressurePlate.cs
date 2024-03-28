using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Choose how the pressure plate is activated: ")]
    [SerializeField] private PlateType plateType;

    [Header("Slime Plate Settings: ")]
    [SerializeField] private int slimesToActivate;
    private int slimesOnPlate = 0;
    private int iceCubesOnPlate = 0;
    private bool plateIsActive;
    private int slimeLayer, iceLayer, colliderLayer;

    private void Awake() {
        slimeLayer = LayerMask.NameToLayer("Slime");
        iceLayer = LayerMask.NameToLayer("Ice");

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

        //if plate is NOT activated and enough slimes are on plate
        if(!plateIsActive && slimesOnPlate >= slimesToActivate) {
            //activate plate
            plateIsActive = true;
            Debug.Log("Plate is activated");
        }

        //if plate IS activated and NOT enough slimes are on plate
        else if(plateIsActive && slimesOnPlate < slimesToActivate) {
            //DEactivate plate
            plateIsActive = false;
            Debug.Log("Plate is deactivated");
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
        }

        //if plate IS activated and NOT enough ice cubes are on plate
        else if(plateIsActive && iceCubesOnPlate < 1) {
            //deactivate plate
            plateIsActive = false;
            Debug.Log("Plate is deactivated");
        }
    }


    //enums are a set of named values
    //PlateType specifies what activates this pressure plate
    private enum PlateType {
        Slime, IceCube
    }

}
