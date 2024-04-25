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
    
    //restricted island trackers
    private bool unlockedFrostbite = false;
    private bool unlockedCoven = false;
    private bool unlockedEngineer = false;
    private bool unlockedOoze = false;

    //keeps Save Manager object active throughout every scene and deletes duplicates
    private void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Save Manager");

        if(objs.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    //Saves the slimes collected if collected more slimes than current best score
    // also since this script is only called when canon is used, it will unlock the next level
    public void SaveSlimeCount(int slimeCount) {

        //Compare name of current scene
        switch (SceneManager.GetActiveScene().name) {

            case "InfernoIsland":
                unlockedFrostbite = true;
                if(infernoSlimesCollected < slimeCount)
                    infernoSlimesCollected = slimeCount;
                break;

            case "FrostbiteIsland":
                unlockedCoven = true;
                if(frostbiteSlimesCollected < slimeCount)
                    frostbiteSlimesCollected = slimeCount;
                break;

            case "CovenIsland":
                unlockedEngineer = true;
                if(covenSlimesCollected < slimeCount)
                    covenSlimesCollected = slimeCount;
                break;

            case "EngineerIsland":
                unlockedOoze = true;
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


    //Eeturns slimeling high score for all islands (used by LevelSelectManager)
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

    //Returns false if level is locked and true if unlocked (used by LevelSelectManager)
    public bool IsIslandUnlocked(char islandChar) {
        
        //Compare character
        switch (islandChar) {

            case 'F':
                return unlockedFrostbite;

            case 'C':
                return unlockedCoven;

            case 'E':
                return unlockedEngineer;

            case 'O':
                return unlockedOoze;

            //else, incorrect input
            default:
                Debug.LogError("IsIslandUnlocked input is wrong in Save Manager script");
                return false;
        }
    }
}
