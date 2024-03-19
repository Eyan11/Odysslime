using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeVitality : MonoBehaviour
{
    [Header("Settings")]
    private bool enableResetKeybind = true; // Press R to die
    private GameObject slimeKing;
    private SlimePossess slimePossess;

    private void Awake() {
        slimeKing = GameObject.FindObjectOfType<KingMovement>().gameObject;
        slimePossess = GetComponent<SlimePossess>();
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

            // Destroys slime
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() {
        if (slimePossess.enabled && Input.GetKey(KeyCode.R) && enableResetKeybind) {
            this.enabled = false;
        }
    }
}
