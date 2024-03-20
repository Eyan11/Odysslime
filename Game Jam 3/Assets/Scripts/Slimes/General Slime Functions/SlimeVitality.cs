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
    private TMP_Text promptText;
    private void Awake() {
        slimeKing = GameObject.FindObjectOfType<KingMovement>().gameObject;
        slimePossess = GetComponent<SlimePossess>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        promptText = GameObject.Find("PromptText").GetComponent<TMP_Text>();
    }

    private void OnDisable() {
        promptText.text = "";

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
        if (slimePossess.enabled && Input.GetKey(KeyCode.R) && enableResetKeybind) {
            this.enabled = false;
        }
    }
}
