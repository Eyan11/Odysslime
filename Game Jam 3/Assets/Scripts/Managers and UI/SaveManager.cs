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
    private int totalInfernoSlimes = 23;
    private int totalFrostbiteSlimes = 20;
    private int totalCovenSlimes = 22;
    private int totalEngineerSlimes = 22;
    private int totalOozeSlimes = 28;
    
    //restricted island trackers
    private bool unlockedFrostbite = false;
    private bool unlockedCoven = false;
    private bool unlockedEngineer = false;
    private bool unlockedOoze = false;

    //cutscene trackers
    private bool seenVolcanoCutscene = false;
    
    //unlock all island cheat
    private InputMap inputMap;
    private bool cheatActivated = false;
    private bool cheatInput = false;
    private float cheatInputTime = 5f;
    private float cheatTimer = 0f;


    //keeps Save Manager object active throughout every scene and deletes duplicates
    private void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Save Manager");

        if(objs.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        //create a new Input Map object and enable the King Slime input
        inputMap = new InputMap();
        inputMap.UI.Enable();
    }

    private void Update() {

        //if cheat has NOT been activated yet
        if(!cheatActivated) {
            //get input
            cheatInput = inputMap.UI.Cheat.triggered;

            //if held cheat input for 5 seconds (through input map interactions)
            if(cheatInput) {
                cheatActivated = true;

                Debug.Log("CHEAT MODE YAY");

                //unlock all islands
                unlockedFrostbite = true;
                unlockedCoven = true;
                unlockedEngineer = true;
                unlockedOoze = true;
            }
        }

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


    //------------------ Getter Method For Level Select to Display Slime Stats -------------------\\
    public string GetIslandSlimeStats(char islandChar) {
        
        //Compare character
        switch (islandChar) {

            case 'I':
                return infernoSlimesCollected + " / " + totalInfernoSlimes;

            case 'F':
                return frostbiteSlimesCollected + " / " + totalFrostbiteSlimes;

            case 'C':
                return covenSlimesCollected + " / " + totalCovenSlimes;

            case 'E':
                return engineerSlimesCollected + " / " + totalEngineerSlimes;

            case 'O':
                return oozeSlimesCollected + " / " + totalOozeSlimes;

            //else, incorrect input
            default:
                Debug.LogError("GetIslandSlimesCollected input is wrong in Save Manager script");
                return "GetIslandSlimesCollected input is wrong in Save Manager script";
        }
    }

    //------------------------ Getter Method For TrophySlimeSpawner -----------------------\\

    //same as above except only returns slimelings collected
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


    //------------------------ Getter/Setter for Cutscenes --------------------------\\

    public bool SeenVolcanoCutscene() {
        return seenVolcanoCutscene;
    }

    public void FinishedVolcanoCutscene() {
        seenVolcanoCutscene = true;
    }
}
