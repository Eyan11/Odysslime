using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillPartCollect : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) {

        if(other.gameObject.CompareTag("Slime Follower") || other.gameObject.CompareTag("Slime King")) {

            //INSERT ADD DRILL PART METHOD CALL HERE

            Destroy(gameObejct);
        }
    }

}
