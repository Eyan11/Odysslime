using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlimeVitality : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip deathSound;
    private bool enableResetKeybind = true; // Press R to die
    private GameObject slimeKing;
    private SlimePossess slimePossess;
    private SoundManager soundManager;
    private UIManager UIScript;
    private bool isQuitting = false;
    private void Awake() {
        slimeKing = GameObject.FindWithTag("King Slime");
        slimePossess = GetComponent<SlimePossess>();
        soundManager = GameObject.FindWithTag("Sound Manager").GetComponent<SoundManager>();
        UIScript = GameObject.FindWithTag("UI Manager").GetComponent<UIManager>();
    }

    private void OnDisable() {
        if (isQuitting) return; // Prevents death sound from being generated on quit

        if (slimeKing) {
            // Return control back to king slime if currently possessed
            if (slimePossess != null && slimePossess.enabled) {
                // Returns control back to slime king
                slimePossess.PosessSlime(slimeKing);
            }

            //if currently following the king (and therefore in slime follower array)
            if(GetComponent<SlimeFollow>().enabled == true)
                //remove this slime from slime follower array
                slimeKing.GetComponent<DiscoverSlimes>().RemoveSlimeFollower(this.gameObject.transform);

            //if this is a slimeling, then update the UI counter
            if(this.gameObject.CompareTag("Slimeling")) {
                UIScript.UpdateSlimelingCount(-1);
            }

            soundManager.PlaySoundEffectAtPoint(deathSound, transform.position, 0.9f);

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

    private void OnApplicationQuit() {
        isQuitting = true;
    }
}
