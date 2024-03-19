using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoverSlimes : MonoBehaviour
{
    [SerializeField] private float searchRadius;
    [SerializeField] private float searchInterval;
    private SlimePossess slimePossessScript;
    private GameObject[] slimeFollowerArray = new GameObject[100];
    private int numSlimeFollowers = 0;
    private float searchCounter = 0;

    private void Awake() {
        slimePossessScript = GetComponent<SlimePossess>();
    }

    private void Update() {
        searchCounter -= Time.deltaTime;

        //search for new slimes if counter is done and currently possessing king
        if(searchCounter < 0 && slimePossessScript.enabled == true)
            SearchForNewSlimes();

    }

    private void SearchForNewSlimes() {
        searchCounter = searchInterval;

        //collect all colliders in radius of king
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius);

        //loop through each collider in array
        foreach (var hitCollider in hitColliders) {

            //if collider belongs to slime follower and slime is not following leader
            if(hitCollider.gameObject.CompareTag("Slime Follower") &&
                hitCollider.gameObject.GetComponent<SlimeFollow>().enabled == false) {

                //make slime a follower
                AddSlimeFollower(hitCollider.gameObject);
            }
        }
    }

    public void AddSlimeFollower(GameObject newFollower) {
        //add slime to array and increase size
        slimeFollowerArray[numSlimeFollowers] = newFollower;
        numSlimeFollowers++;

        //allow slime to follow king
        newFollower.GetComponent<SlimeFollow>().enabled = true;
    }

    public void RemoveSlimeFollower(GameObject removedFollower) {
        //find index of slime
        int index = FindSlimeFollower(removedFollower);

        //if slime isn't a follower
        if(index == -1)
            return;

        //remove slime and update size
        slimeFollowerArray[index] = null;
        numSlimeFollowers--;

        //if dead slime was NOT last element in array
        if(index != numSlimeFollowers) {
            //move last element into empty slot
            slimeFollowerArray[index] = slimeFollowerArray[numSlimeFollowers];
            //make old spot null
            slimeFollowerArray[numSlimeFollowers] = null;
        }

    }

    //returns index of slime follower, if not found returns -1
    public int FindSlimeFollower(GameObject slime) {

        //check every slime follower
        for(int i = 0; i <numSlimeFollowers; i++) {

            //if found the same slime, return it's index in array
            if(slimeFollowerArray[i].GetInstanceID() == slime.GetInstanceID())
                return i;
        }

        //slime not found
        return -1;
    }

    public GameObject GetSlimeFollower() {
        return slimeFollowerArray[0];
    }

}

