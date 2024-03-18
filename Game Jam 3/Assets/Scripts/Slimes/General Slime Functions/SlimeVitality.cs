using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeVitality : MonoBehaviour
{
    private GameObject slimeKing;
    private SlimePossess slimePossess;

    private void Awake() {
        slimeKing = GameObject.FindObjectOfType<KingMovement>().gameObject;
        slimePossess = GetComponent<SlimePossess>();
    }

    private void OnDisable() {
        if (slimeKing) {
            // Returns control back to slime king
            slimePossess.PosessSlime(slimeKing);

            // Destroys slime
            Destroy(gameObject);
        }
    }
}
