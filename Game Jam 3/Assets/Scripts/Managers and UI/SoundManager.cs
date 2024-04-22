using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine; 

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource BGMSource;

    [Header("Sounds")]
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip eatSound;
    [SerializeField] private AudioClip freezeSound;
    [SerializeField] private AudioClip slimeMoveSound;
    [SerializeField] private AudioClip slimeJumpSound;
    [SerializeField] private AudioClip slimeDeathSound;
    [SerializeField] private AudioClip buttonSelectSound;

    [Header("Music")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip worldMusic;


    //Not making this object persistent as of now.
    //I will update this script in week 3/4
    /*
    //deletes all other sound manager objects so that there is only 1 in scene
    private void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Sound Manager");

        //only keep the first sound manager instantiated in scene
        if(objs.Length > 1)
            Destroy(this.gameObject);

        //stays from scene to scene
        DontDestroyOnLoad(this.gameObject);
    }
    */

    // Plays a sound at a location with a customizable volume
    // The sound automatically cleans itself
    public void PlaySoundEffectAtPoint(AudioClip audioClip, UnityEngine.Vector3 point, float volume) {
        AudioSource.PlayClipAtPoint(audioClip, point, volume);
    }

    // Plays a sound at a location using the default volume of 0.6f
    public void PlaySoundEffectAtPoint(AudioClip audioClip, UnityEngine.Vector3 point) {
        PlaySoundEffectAtPoint(audioClip, point, 0.6f);
    }

    // Plays looping background music using a customizable volume
    public void PlayBackgroundMusic(AudioClip audioClip, float volume) {
        BGMSource.PlayOneShot(audioClip, volume);
    }

    // Plays looping ackground music using the default volume of 0.5f;
    public void PlayBackgroundMusic(AudioClip audioClip) {
        PlayBackgroundMusic(audioClip, 0.5f);
    }

    public void PlayExplosion() {
        //second argument is priority level of sound (0-1)
        if (!source) return;
        source.PlayOneShot(explosionSound, 0.6f);
    }

    public void PlaySlimeFreeze() {
        //second argument is priority level of sound (0-1)
        if (!source) return;
        source.PlayOneShot(freezeSound, 0.6f);
    }

    public void PlaySlimeEat() {
        //second argument is priority level of sound (0-1)
        if (!source) return;
        source.PlayOneShot(eatSound, 0.6f);
    }

    public void PlaySlimeMove() {
        //second argument is priority level of sound (0-1)
        if (!source) return;
        source.PlayOneShot(slimeMoveSound, 0.4f);
    }

    public void PlaySlimeJump() {
        //second argument is priority level of sound (0-1)
        if (!source) return;
        source.PlayOneShot(slimeJumpSound, 0.5f);
    }

    public void PlaySlimeDeath() {
        //second argument is priority level of sound (0-1)
        if (!source) return;
        source.PlayOneShot(slimeDeathSound, 0.7f);
    }

    public void PlayButtonSelect() {
        //second argument is priority level of sound (0-1)
        if (!source) return;
        source.PlayOneShot(buttonSelectSound, 0.7f);
    }

    public void PlayWorldMusic() {
        //second argument is priority level of sound (0-1)
        if (!musicSource) return;
        musicSource.Stop();
        musicSource.PlayOneShot(worldMusic, 0.5f);
    }

    public void PlayMenuMusic() {
        //second argument is priority level of sound (0-1)
        if (!musicSource) return;
        musicSource.Stop();
        musicSource.PlayOneShot(menuMusic, 0.5f);
    }

}
