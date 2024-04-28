using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource globalSource;
    [SerializeField] private AudioSource BGMSource;

    [Header("Volume Defaults")]
    [SerializeField] private float soundEffectVolume = 0.6f;
    [SerializeField] private float backgrondMusicVolume = 0.5f;

    [Header("Background Noise")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip ambienceMusic;
    private PauseMenuManager pauseMenuManager;
    //Volume Settings
    private float savedMusicPercent = 1; //from SaveManager, percent from 0-1
    private float savedSFXPercent = 1; //from SaveManager, percent from 0-1

    private void Awake() {
        // Plays music if in the settings
        if (backgroundMusic) {
            PlayBackgroundMusic(backgroundMusic, 0.4f * savedMusicPercent);
        }
        if (ambienceMusic) {
            PlayBackgroundMusic(ambienceMusic, 0.1f * savedMusicPercent);
        }

        // Retrieves pauseMenuManager
        pauseMenuManager = FindObjectOfType<PauseMenuManager>();
    }


    private void Update() {
        if (pauseMenuManager != null && pauseMenuManager.IsPaused()) {
            BGMSource.volume = 0.1f;
        } else {
            BGMSource.volume = 1.0f;
        }
    }

    // Plays a sound at a location with a customizable volume
    public void PlaySoundEffectAtPoint(AudioClip audioClip, UnityEngine.Vector3 point, float volume) {
        HasAudioClip(audioClip);
        
        AudioSource.PlayClipAtPoint(audioClip, point, volume * savedSFXPercent);
    }

    // Plays a sound at a location using the default sound effect volume
    public void PlaySoundEffectAtPoint(AudioClip audioClip, UnityEngine.Vector3 point) {
        PlaySoundEffectAtPoint(audioClip, point, soundEffectVolume * savedSFXPercent);
    }

    // Plays a sound on an object with a customizable volume
    public AudioSource PlaySoundEffectOnObject(AudioClip audioClip, GameObject objectToPlayOn, float volume) {
        HasAudioClip(audioClip);
        
        AudioSource audioSource = objectToPlayOn.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volume * savedSFXPercent;
        audioSource.Play();
        Destroy(audioSource, audioClip.length);

        return audioSource;
    }

    // Plays a sound on an object with the default sound effect volume
    public AudioSource PlaySoundEffectOnObject(AudioClip audioClip, GameObject objectToPlayOn) {
        return PlaySoundEffectOnObject(audioClip, objectToPlayOn, soundEffectVolume * savedSFXPercent);
    }

    // Plays a sound globally with a customizable volume
    public void PlayGlobalSoundEffect(AudioClip audioClip, float volume) {
        HasAudioClip(audioClip);
        
        globalSource.PlayOneShot(audioClip, volume * savedSFXPercent);
    }

    // Plays a sound globally with a default sound effect volume
    public void PlayGlobalSoundEffect(AudioClip audioClip) {
        PlayGlobalSoundEffect(audioClip, soundEffectVolume * savedSFXPercent);
    }
    

    // Plays looping background music using a customizable volume
    public void PlayBackgroundMusic(AudioClip audioClip, float volume) {
        HasAudioClip(audioClip);

        BGMSource.PlayOneShot(audioClip, volume * savedMusicPercent);
    }

    // Plays looping ackground music using the default background music volume
    public void PlayBackgroundMusic(AudioClip audioClip) {
        PlayBackgroundMusic(audioClip, backgrondMusicVolume * savedMusicPercent);
    }

    private void HasAudioClip(AudioClip audioClip) {
        if (!audioClip) {
            Debug.LogError("No audio clip found!");
        }
    }

    // --------------------- Sound Settings ------------------- \\

    public void SetVolume(float musicVolume, float sfxVolume) {
        savedMusicPercent = musicVolume;
        savedSFXPercent = sfxVolume;
    }

}
