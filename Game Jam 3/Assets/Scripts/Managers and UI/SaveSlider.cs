using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlider : MonoBehaviour
{
    [Header("Game Settings References")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider sensSlider;
    [SerializeField] private SoundManager soundScript;
    
    //Saved game settings (settings are float percent from 0-1)
    private float musicVolume;
    private float sfxVolume;
    private float sensitivity;

    private void Start() {
        //if no current save data for music volume, set default music volume to 80%
        if(!PlayerPrefs.HasKey("Music Volume"))
            PlayerPrefs.SetFloat("Music Volume", 0.8f);

        //if no current save data for sfx volume, set default sfx volume to 80%
        if(!PlayerPrefs.HasKey("SFX Volume")) 
            PlayerPrefs.SetFloat("SFX Volume", 0.8f);

        //if no current save data for sens, set default sens to 50%
        if(!PlayerPrefs.HasKey("Sensitivity")) 
            PlayerPrefs.SetFloat("Sensitivity", 0.5f);
        
        //set sliders to default values
        LoadSettings();
    }

    //Called when any game options slider values are changed
    public void SliderValueChange() {
        //Save new slider values in SaveManager script
        musicVolume = musicSlider.value;
        sfxVolume = sfxSlider.value;
        sensitivity = sensSlider.value;
        SaveSettings();
    }

    public void LoadSettings() {
        musicSlider.value = PlayerPrefs.GetFloat("Music Volume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFX Volume");
        sensSlider.value = PlayerPrefs.GetFloat("Sensitivity");
        Debug.Log("Load Settings: ");
    }

    private void SaveSettings() {
        //save slider values to player computer
        PlayerPrefs.SetFloat("Music Volume", musicVolume);
        PlayerPrefs.SetFloat("SFX Volume", sfxVolume);
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);

        //Debug.Log("Save Settings");

        //Actually apply sound volume in SoundManager
        soundScript.SetVolume(musicVolume, sfxVolume);
    }
}
