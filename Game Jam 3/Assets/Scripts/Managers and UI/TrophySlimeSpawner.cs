using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrophySlimeSpawner : MonoBehaviour
{
    [Header ("Island Initial (I, F, C, E, O)")]
    [SerializeField] private char islandChar;

    [Header("Slimeling Prefabs")]
    [SerializeField] private List<GameObject> slimePrefabList;
    private int slimePrefabSize;
    private Transform[] spawnPositions;
    private SaveManager saveScript;
    private int slimesCollected;
    private int numSlimePositions;
    private Vector3 spawnPos;
    private GameObject slimeInstance;

    private void Awake() {
        saveScript = GameObject.FindWithTag("Save Manager").GetComponent<SaveManager>();
        slimePrefabSize = slimePrefabList.Count;
    }
    private void Start() {
        //get the transform component of all children
        spawnPositions = GetComponentsInChildren<Transform>(true);
        numSlimePositions = spawnPositions.Length;
        Debug.Log("numSlimePositions" + numSlimePositions);

        //get slimes collected and make sure there are enough spawn positions
        slimesCollected = saveScript.GetIslandSlimesCollected(islandChar);

        if(slimesCollected > numSlimePositions) {
            Debug.LogError("Need to place more spawner positions for island char: " + islandChar);
            return;
        }

        //for every slimeling collected in island
        for(int i = 0; i < slimesCollected; i++) {

            //set SpawnPos to spawnPosition plus a random XZ plane offset between -1 and 1
            spawnPos = spawnPositions[i].position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));

            //spawn a random slime in slimePrefabList at spawnPos
            slimeInstance = Instantiate(slimePrefabList[Random.Range(0, slimePrefabSize-1)], spawnPos, transform.rotation);

            //Stop all slimeling movement
            slimeInstance.GetComponent<NavMeshAgent>().speed = 0f;
        }

    }

}
