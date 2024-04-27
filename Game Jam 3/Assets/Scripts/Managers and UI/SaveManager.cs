using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    //Slimes collected trackers
    private int infernoSlimesCollected = 0;
    private int frostbiteSlimesCollected = 0;
    private int covenSlimesCollected = 0;
    private int engineerSlimesCollected = 0;
    private int oozeSlimesCollected = 0;

    //Total slimes trackers
    private int totalInfernoSlimes = 0;
    private int totalFrostbiteSlimes = 0;
    private int totalCovenSlimes = 0;
    private int totalEngineerSlimes = 0;
    private int totalOozeSlimes = 0;
    
    //restricted island trackers
    private bool unlockedFrostbite = true;
    private bool unlockedCoven = true;
    private bool unlockedEngineer = true;
    private bool unlockedOoze = true;

    //cutscene trackers
    private bool seenVolcanoCutscene = false;

    //keeps Save Manager object active throughout every scene and deletes duplicates
    private void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Save Manager");

        if(objs.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    //------------------------ Setter Method for Island Info Upon Using Canon --------------------------\\

    //Saves the slimes collected if collected more slimes than current best score
    // also since this script is only called when canon is used, it will unlock the next level
    public void SaveSlimeCount(int slimesCollected, int totalSlimes) {

        //Compare name of current scene
        switch (SceneManager.GetActiveScene().name) {

            case "InfernoIsland":
                unlockedFrostbite = true;
                totalInfernoSlimes = totalSlimes;
                if(infernoSlimesCollected < slimesCollected)
                    infernoSlimesCollected = slimesCollected;
                break;

            case "FrostbiteIsland":
                unlockedCoven = true;
                totalFrostbiteSlimes = totalSlimes;
                if(frostbiteSlimesCollected < slimesCollected)
                    frostbiteSlimesCollected = slimesCollected;
                break;

            case "CovenIsland":
                unlockedEngineer = true;
                totalCovenSlimes = totalSlimes;
                if(covenSlimesCollected < slimesCollected)
                    covenSlimesCollected = slimesCollected;
                break;

            case "EngineerIsland":
                unlockedOoze = true;
                totalEngineerSlimes = totalSlimes;
                if(engineerSlimesCollected < slimesCollected)
                    engineerSlimesCollected = slimesCollected;
                break;

            case "Ooze Island":
                totalOozeSlimes = totalSlimes;
                if(oozeSlimesCollected < slimesCollected)
                    oozeSlimesCollected = slimesCollected;
                break;

            //else
            default:
                Debug.LogError("Island name is wrong in Save Manager script, or trying to save slime count from a non-island scene");
                break;
        }
    }


    //------------------------ Getter Method Island Slimeling Count States --------------------------\\
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

    //------------------------ Getter Method Island Lock/Unlock States --------------------------\\
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


    //------------------------ Getter Method for Cutscenes --------------------------\\

    public bool SeenVolcanoCutscene() {
        return seenVolcanoCutscene;
    }


    //------------------------ Setter Method for Cutscenes --------------------------\\

    public void FinishedVolcanoCutscene() {
        seenVolcanoCutscene = true;
    }
    
}
