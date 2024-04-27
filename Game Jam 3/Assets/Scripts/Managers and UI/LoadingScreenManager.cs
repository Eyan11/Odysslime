using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    [Header ("Loading Screen Images")]
    [SerializeField] private List<GameObject> imageList;

    [Header ("References (Ignore For Cannon Loading Screen)")]
    [SerializeField] private MainMenuManager mainMenuScript;

    [Header ("Settings")]
    [SerializeField] private string sceneNameToLoad;
    [SerializeField] private float timePerImage;
    private SaveManager saveScript;
    private AsyncOperation asyncOperation;
    private bool isSceneLoaded = false;
    private bool hasWatchedLoadingScreen = false;
    private int imageIndex = 0;
    private float imageCountdown = 0;

    private void Awake() {
        saveScript = GameObject.FindWithTag("Save Manager").GetComponent<SaveManager>();
    }

    private void Update() {
        LoadingScreenController();
    }

    private void LoadingScreenController() {

        //if haven't finished watching loading screen
        if(hasWatchedLoadingScreen == false) {
            //counts down even when paused
            imageCountdown -= Time.unscaledDeltaTime;

            //if done watching image and on final image
            if(imageCountdown < 0 && imageIndex >= imageList.Count - 1)
                hasWatchedLoadingScreen = true;
            
            //if done watching image and NOT on final image
            else if(imageCountdown < 0) {
                imageCountdown = timePerImage;

                //enable next cutscene, disable previous cutscene
                imageIndex++;
                imageList[imageIndex].SetActive(true);
                imageList[imageIndex - 1].SetActive(false);
            }
        }
        //if on finished watching loading screen AND scene finsihed loading, play next scene
        else if(isSceneLoaded)
            asyncOperation.allowSceneActivation = true;
    }


    // ----------------------- Methods that Change Scenes -------------------- \\

    private IEnumerator LoadSceneCoroutine(string sceneName) {

        //start loading next scene 
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        //don't activate next scene yet
        asyncOperation.allowSceneActivation = false;

        //while scene is still loading
        while(asyncOperation.progress < 0.9f) {

            //Debug.Log("Loading Progress: " + asyncOperation.progress);
        }

        //scene is done loading when progress is at 90%
        isSceneLoaded = true;
        //don't pause coroutine
        yield return null;
    }

    private void OnEnable() {

        //disable all but the first cutscene
        for(int i = 1; i < imageList.Count; i++) {
            imageList[i].SetActive(false);
        }
        //enable first cutscene
        imageList[0].SetActive(true);

        //reset cutscene index and timer
        imageIndex = 0;
        imageCountdown = timePerImage;

        //if in MainMenu scene, disable main menu input
        if(sceneNameToLoad != "MainMenu")
            mainMenuScript.OnLoadingScreen();
        
        //calls LoadSceneCoroutine which loads next scene in background
        StartCoroutine(LoadSceneCoroutine(sceneNameToLoad));
    }
}
