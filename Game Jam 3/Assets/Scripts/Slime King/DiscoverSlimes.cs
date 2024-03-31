using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoverSlimes : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float searchRadius;
    [SerializeField] private float searchInterval;
    [SerializeField] private float findClosestSlimeInterval;
    private float findClosestSlimeCountdown = 0;
    private KingMovement moveScript;
    private SlimePossess possessScript;
    private int slimeLayer;
    private Transform[] followerArray = new Transform[100];
    private Transform closestSlime;
    private int numFollowers = 0;
    private float searchCounter = 0;

    private void Awake() {
        possessScript = GetComponent<SlimePossess>();
        moveScript = GetComponent<KingMovement>();

        //get layer of slimes and convert to a bitmask
        slimeLayer = 1<<LayerMask.NameToLayer("Slime");
    }

    private void Update() {
        searchCounter -= Time.deltaTime;
        findClosestSlimeCountdown -= Time.deltaTime;

        //search for new slimes if counter is done and currently possessing king
        if(searchCounter < 0 && possessScript.enabled == true)
            SearchForNewSlimes();

        if(findClosestSlimeCountdown < 0) {
            CalculateClosestFollower();
        }
    }

    private void SearchForNewSlimes() {
        searchCounter = searchInterval;

        //collect all colliders with slime layer in radius of the floor directly below King
        Collider[] hitColliders = Physics.OverlapSphere(moveScript.RayCastDownPosition(), searchRadius, slimeLayer);

        //loop through each collider in array
        foreach (var hitCollider in hitColliders) {
            
            //if not King and not currently following King
            if(hitCollider.gameObject != gameObject && 
                hitCollider.gameObject.GetComponent<SlimeFollow>().enabled == false) {

                //make slime a follower
                AddSlimeFollower(hitCollider.transform);
                //make slime follow King
                hitCollider.gameObject.GetComponent<SlimeFollow>().enabled = true;
            }
            
        }
    }

    public void AddSlimeFollower(Transform newFollower) {
        //add slime to array and increase size
        followerArray[numFollowers] = newFollower;
        numFollowers++;

        //Debug.Log("Added Follower: " + newFollower.gameObject.name);

        //update closest slime follower
        CalculateClosestFollower();

    }

    public void RemoveSlimeFollower(Transform removedFollower) {
        //find index of slime
        int index = FindSlimeFollower(removedFollower);

        //if slime isn't a follower, don't remove it
        if(index == -1)
            return;

        //remove slime and update size
        followerArray[index] = null;
        numFollowers--;

        //if dead slime was NOT last element in array
        if(index != numFollowers) {
            //move last element into empty slot
            followerArray[index] = followerArray[numFollowers];
            //make old spot null
            followerArray[numFollowers] = null;
        }

        //update closest slime follower
        CalculateClosestFollower();

        //Debug.Log("Removed Follower: " + removedFollower.gameObject.name);
    }

    //returns index of given slime follower, if not found returns -1
    public int FindSlimeFollower(Transform slime) {

        //check every slime follower
        for(int i = 0; i < numFollowers; i++) {

            //if found the same slime, return it's index in array
            if(followerArray[i].gameObject.GetInstanceID() == slime.gameObject.GetInstanceID())
                return i;
        }

        //slime not found
        return -1;
    }

    private void CalculateClosestFollower() {
        findClosestSlimeCountdown = findClosestSlimeInterval;

        //if no followers, return vector (0,0,0) to represent null
        if(numFollowers == 0) {
            closestSlime = null;
            moveScript.SetClosestSlime(closestSlime);
            return;
        }
        //if only one follower, return its position
        else if(numFollowers == 1) {
            closestSlime = followerArray[0];
            moveScript.SetClosestSlime(closestSlime);
            return;
        }
        
        //declare variables to store info
        float curSlimeDistance = 0f;
        float closestSlimeDistance = Mathf.Infinity;
        int closestSlimeIndex = -1;

        //check every slime follower
        for(int i = 0; i < numFollowers; i++) {

            //check the slime's distance to the King
            curSlimeDistance = Vector3.Distance(transform.position, followerArray[i].position);
            
            //if slime is the closest to King so far, collect its distance and index in array
            if(curSlimeDistance < closestSlimeDistance) {
                closestSlimeDistance = curSlimeDistance;
                closestSlimeIndex = i;
            }  
        }
        //set the closest slime follower
        closestSlime = followerArray[closestSlimeIndex];
        //update closest slime follower on move script
        moveScript.SetClosestSlime(closestSlime);
    }

}

