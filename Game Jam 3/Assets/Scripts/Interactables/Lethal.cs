using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Lethal : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider collider) {
        GameObject colliderObj = collider.gameObject;

        // Checks if its a slime thats killable
        SlimeVitality slimeVitality = colliderObj.GetComponent<SlimeVitality>();
        if (!slimeVitality) { return; }

        slimeVitality.enabled = false;
    }
}
