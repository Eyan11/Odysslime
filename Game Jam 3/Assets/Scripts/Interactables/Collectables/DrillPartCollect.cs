using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillPartCollect : MonoBehaviour
{
    private UIManager UIScript;
    private void Awake() {
        //TMP reference for UI
        UIScript = GameObject.FindWithTag("UI Manager").GetComponent<UIManager>();
    }

    private void OnTriggerEnter(Collider other) {

        if(other.gameObject.CompareTag("Slime Follower") || other.gameObject.CompareTag("Slime King")) {

            //INSERT ADD DRILL PART METHOD CALL HERE
            UIScript.AddDrillPart();

            Destroy(gameObject);
        }
    }

}
