using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoveSlime : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Super Slime") || other.gameObject.CompareTag("Slimeling")) {
            //add to list
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Super Slime") || other.gameObject.CompareTag("Slimeling")) {
            //remove from list
        }
    }

}
