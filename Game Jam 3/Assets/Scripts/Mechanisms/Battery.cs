using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MechanismBase mechanism;
    [Header("Settings")]
    [SerializeField] private int chargesNeeded = 1;
    // can be changed before runtime to have some initial charge
    [Header("Read only!")]
    [SerializeField] private int currentCharges = 0; 
    private int chargesHash;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();

        if (mechanism) {
            CheckCharge();
            // Gets int hash for animator int variable "Charges"
            chargesHash = Animator.StringToHash("ChargesDifference");
        } else {
            Debug.LogError("Missing a mechanism reference!");
            this.enabled = false;
        }
    }

    public bool IsFullyCharged() {
        return currentCharges >= chargesNeeded;
    }

    private void CheckCharge()
    {
        int chargeDif = Math.Clamp(chargesNeeded - currentCharges, 0, 3);
        animator.SetInteger("ChargesDifference", chargeDif);
        // Activates mechanism if enouch charge
        if (chargeDif != 0) { return; }

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
