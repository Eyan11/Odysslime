using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlimeVitality : MonoBehaviour
{
    [Header("Settings")]
    private bool enableResetKeybind = true; // Press R to die
    private GameObject slimeKing;
    private SlimePossess slimePossess;
    private SoundManager soundManager;
    private void Awake() {
        slimeKing = GameObject.FindObjectOfType<KingMovement>().gameObject;
        slimePossess = GetComponent<SlimePossess>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();
    }

    private void OnDisable() {

        if (slimeKing) {
            // Return control back to king slime if currently possessed
            if (slimePossess.enabled) {
                // Returns control back to slime king
                slimePossess.PosessSlime(slimeKing);
            }

            //if currently following the king (and therefore in slime follower array)
            if(GetComponent<SlimeFollow>().enabled == true)
                //remove this slime from slime follower array
                slimeKing.GetComponent<DiscoverSlimes>().RemoveSlimeFollower(this.gameObject);

            soundManager.PlaySlimeDeath();

            // Destroys slime
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() {
        // This is used to check if the slime is possessable
        if (!slimePossess) {
            return;
        }

        // If it is and it is currently in possession and can do the debug reset, then kill the slime
        if (slimePossess.enabled && Input.GetKey(KeyCode.R) && enableResetKeybind) {
            this.enabled = false;
        }
    }
}
