using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SaveSlider : MonoBehaviour
{
    [Header("Game Settings References")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider sensSlider;
    [SerializeField] private SoundManager soundScript;
    private ThirdPersonCam camScript;

    //ignore slider variables
    private float ignoreSliderTimer = 1f;
    private bool allowSliderChange = false;
    
    //Saved game settings (settings are float percent from 0-1)
    private float savedVolume;
    private float savedSens;

    //SliderValueChange is called when game starts by unity, this timer is to ignore the call
    private void Update() {
        //if slider change is NOT allowed
        if(!allowSliderChange) {
            //countdown from 1 second
            ignoreSliderTimer-= Time.unscaledDeltaTime;

            //if timer is finished, allow slider to be changed
            if(ignoreSliderTimer < 0)
                allowSliderChange = true;
        }
    }

    private void Start() {
        //if no current save data for music volume, set default music volume to 80%
        if(!PlayerPrefs.HasKey("Volume"))
            PlayerPrefs.SetFloat("Volume", 0.8f);

        //if no current save data for sens, set default sens to 50%
        if(!PlayerPrefs.HasKey("Sensitivity")) 
            PlayerPrefs.SetFloat("Sensitivity", 0.5f);

        //get camera script (is null for main menu)
        camScript = Camera.main.GetComponent<ThirdPersonCam>();
        
        //set sliders to default values
        LoadSettings();
        //apply saved settings to game
        SaveSettings();
    }

    //Called when any game options slider values are changed
    public void SliderValueChange() {
        //if the scene has been active for less than 1 second, ignore call
        if(!allowSliderChange)
            return;

        //Save new slider values in SaveManager script
        savedVolume = musicSlider.value;
        savedSens = sensSlider.value;
        SaveSettings();
    }


    public void LoadSettings() {
        musicSlider.value = PlayerPrefs.GetFloat("Volume");
        savedVolume = PlayerPrefs.GetFloat("Volume");

        sensSlider.value = PlayerPrefs.GetFloat("Sensitivity");
        savedSens = PlayerPrefs.GetFloat("Sensitivity");
    }

    private void SaveSettings() {
        //save slider values to player computer
        PlayerPrefs.SetFloat("Volume", savedVolume);
        PlayerPrefs.SetFloat("Sensitivity", savedSens);

        //set entire game volume
        AudioListener.volume = savedVolume;
        //apply sensitivity to camera (not for mouse, just camera)
        if(camScript != null)
            camScript.SaveSensitivitySetting(savedSens);
    }
}
