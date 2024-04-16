using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    [SerializeField] private MechanismBase mechanism;
    [SerializeField] private int chargesNeeded = 1;
    // can be changed before runtime to have some initial charge
    [Header("Read only!")]
    [SerializeField] private int currentCharges = 0; 

    private void Awake() {
        if (mechanism) {
            if (chargesNeeded <= currentCharges) {
                mechanism.Activate();
            }
        } else {
            Debug.LogError("Missing a mechanism reference!");
        }
    }

    private void CheckCharge()
    {
        // Activates mechanism if enouch charge
        if (currentCharges < chargesNeeded) { return; }

        mechanism.Activate();
    }

    public void IncreaseCharge() {
        if (currentCharges < chargesNeeded) {
            currentCharges++;

            // Checks charge to see if it can activate mechanism
            CheckCharge();
        }
    }
}
