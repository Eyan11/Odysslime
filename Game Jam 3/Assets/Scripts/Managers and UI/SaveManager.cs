using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    //Slime stat trackers
    private int infernoSlimesCollected = 0;
    private int frostbiteSlimesCollected = 0;
    private int covenSlimesCollected = 0;
    private int engineerSlimesCollected = 0;
    private int oozeSlimesCollected = 0;

    //keeps Save Manager object active throughout every scene and deletes duplicates
    private void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Save Manager");

        if(objs.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    //Saves the slimes collected if collected more slimes than current best score
    public void SaveSlimeCount(int slimeCount) {

        //Compare name of current scene
        switch (SceneManager.GetActiveScene().name) {

            case "InfernoIsland":
                if(infernoSlimesCollected < slimeCount)
                    infernoSlimesCollected = slimeCount;
                break;

            case "FrostbiteIsland":
                if(frostbiteSlimesCollected < slimeCount)
                    frostbiteSlimesCollected = slimeCount;
                break;

            case "CovenIsland":
                if(covenSlimesCollected < slimeCount)
                    covenSlimesCollected = slimeCount;
                break;

            case "EngineerIsland":
                if(engineerSlimesCollected < slimeCount)
                    engineerSlimesCollected = slimeCount;
                break;

            case "Ooze Island":
                if(oozeSlimesCollected < slimeCount)
                    oozeSlimesCollected = slimeCount;
                break;

            //else
            default:
                Debug.LogError("Island name is wrong in Save Manager script, or trying to save slime count from a non-island scene");
                break;
        }
    }


    //Getter method for all islands (used by LevelSelectManager)
    public int GetIslandSlimesCollected(char islandChar) {
        
        //Compare character
        switch (islandChar) {

            case 'I':
                return infernoSlimesCollected;

            case 'F':
                return frostbiteSlimesCollected;

            case 'C':
                return covenSlimesCollected;

            case 'E':
                return engineerSlimesCollected;

            case 'O':
                return oozeSlimesCollected;

            //else, incorrect input
            default:
                Debug.LogError("GetIslandSlimesCollected input is wrong in Save Manager script");
                return -1;
        }
    }
}
