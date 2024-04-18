using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoverSlimes : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float discoverRadius;
    [SerializeField] private float undiscoverRadius;
    [SerializeField] private float searchInterval;
    [SerializeField] private float updateFollowersInterval;
    private UIManager UIScript;
    private KingMovement moveScript;
    private SlimePossess possessScript;
    private List<Transform> followerList = new List<Transform>(30);
    private List<Transform> undiscoverList = new List<Transform>(15);
    private float updateFollowersCountdown = 0;
    private float searchCountdown = 0;
    private int slimeLayer;

    private void Awake() {
        possessScript = GetComponent<SlimePossess>();
        moveScript = GetComponent<KingMovement>();
        UIScript = GameObject.FindWithTag("UI Manager").GetComponent<UIManager>();

        //get layer of slimes and convert to a bitmask
        slimeLayer = 1<<LayerMask.NameToLayer("Slime");
    }

    private void Update() {
        searchCountdown -= Time.deltaTime;
        updateFollowersCountdown -= Time.deltaTime;

        //search for new slimes on interval
        if(searchCountdown < 0)
            SearchForNewSlimes();

        //search for closest slime on interval
        if(updateFollowersCountdown < 0) {
            UpdateFollowerList();
        }
    }

    private void SearchForNewSlimes() {
        searchCountdown = searchInterval;

        //collect all colliders with slime layer in radius of the floor directly below King
        Collider[] hitColliders = Physics.OverlapSphere(moveScript.RayCastDownPosition(), discoverRadius, slimeLayer);

        //loop through each collider in array
        foreach (var hitCollider in hitColliders) {
            
            //if collider is King, ignore collider
            if(hitCollider.gameObject.CompareTag("King Slime"))
                continue;

            //else if Super Slime and currently being possessed, ignore collider
            else if(hitCollider.gameObject.CompareTag("Super Slime") &&
                    hitCollider.gameObject.GetComponent<SlimePossess>().enabled == true) {
                    continue;
            }

            //else if not a slime follower, add to slime follower list
            else if(hitCollider.gameObject.GetComponent<SlimeFollow>().enabled == false)
                AddSlimeFollower(hitCollider.transform);
        }
    }

    public void AddSlimeFollower(Transform newFollower) {

        followerList.Add(newFollower);
        //make slime follow King
        newFollower.GetComponent<SlimeFollow>().enabled = true;

        //if a slimeling is discovered, update UI counter
        if(newFollower.CompareTag("Slimeling")) {
            UIScript.UpdateSlimelingCount(1);
        }

        //Debug.Log("Added Follower: " + newFollower.gameObject.name);
    }

    public void RemoveSlimeFollower(Transform removedFollower) {

        //if follower was successfully removed from list
        if(followerList.Remove(removedFollower) == true) {

            //make slime stop following King
            removedFollower.GetComponent<SlimeFollow>().enabled = false;
            //Debug.Log("Removed Follower: " + removedFollower.gameObject.name);

            //if removed follower was Slimeling, update UI
            if(removedFollower.CompareTag("Slimeling")) {
                UIScript.UpdateSlimelingCount(-1);
            }
        }
    }

    public bool FindSlimeFollower(Transform targetFollower) {

        //returns true if targetfollower is in the list
        return followerList.Contains(targetFollower);
    }

    //Undiscovers slimes that are too far from King
    private void UpdateFollowerList() {
        
        //declare variables to store info
        float curSlimeDistance = 0f;

        //check every slime follower
        foreach(Transform follower in followerList) {

            //check the slime's distance to the King
            curSlimeDistance = Vector3.Distance(transform.position, follower.position);

            //if slimes are too far away to follow King
            if(curSlimeDistance > undiscoverRadius) {
                
                //put slime in list to undiscover
                undiscoverList.Add(follower);
            }
        }

        //check every soon to be undiscovered slime
        foreach(Transform slime in undiscoverList) {

            //remove from follower list
            RemoveSlimeFollower(slime);
        }

        //remove all elements from list
        undiscoverList.Clear();

        //reset timer
        updateFollowersCountdown = updateFollowersInterval;
    }

}

