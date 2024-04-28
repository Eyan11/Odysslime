using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake() {
        saveScript = GameObject.FindWithTag("Save Manager").GetComponent<SaveManager>();
        slimePrefabSize = slimePrefabList.Count;
    }
    private void Start() {
        //get the transform component of all children
        spawnPositions = GetComponentsInChildren<Transform>(true);
        numSlimePositions = spawnPositions.Length;

        //get slimes collected and make sure there are enough spawn positions
        slimesCollected = saveScript.GetIslandSlimesCollected(islandChar);
        if(slimesCollected > numSlimePositions) {
            Debug.LogError("Need to place more spawner positions for island char: " + islandChar);
            return;
        }

        //for every slimeling collected in island
        for(int i = 0; i < slimesCollected; i++) {

            //spawn a random slime in slimePrefabList at the position of spawnPositions array
            Instantiate(slimePrefabList[Random.Range(0, slimePrefabSize-1)], spawnPositions[i].position, transform.rotation);
        }

    }

}
