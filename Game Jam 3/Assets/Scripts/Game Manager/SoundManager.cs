using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioSource musicSource;

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




    //deletes all other sound manager objects so that there is only 1 in scene
    private void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Sound Manager");

        //only keep the first sound manager instantiated in scene
        if(objs.Length > 1)
            Destroy(this.gameObject);

        //stays from scene to scene
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayExplosion() {
        //second argument is priority level of sound (0-1)
        source.PlayOneShot(explosionSound, 0.6f);
    }

    public void PlaySlimeFreeze() {
        //second argument is priority level of sound (0-1)
        source.PlayOneShot(freezeSound, 0.6f);
    }

    public void PlaySlimeEat() {
        //second argument is priority level of sound (0-1)
        source.PlayOneShot(eatSound, 0.6f);
    }

    public void PlaySlimeMove() {
        //second argument is priority level of sound (0-1)
        source.PlayOneShot(slimeMoveSound, 0.4f);
    }

    public void PlaySlimeJump() {
        //second argument is priority level of sound (0-1)
        source.PlayOneShot(slimeJumpSound, 0.5f);
    }

    public void PlaySlimeDeath() {
        //second argument is priority level of sound (0-1)
        source.PlayOneShot(slimeDeathSound, 0.7f);
    }

    public void PlayButtonSelect() {
        //second argument is priority level of sound (0-1)
        source.PlayOneShot(buttonSelectSound, 0.7f);
    }

    public void PlayWorldMusic() {
        //second argument is priority level of sound (0-1)
        musicSource.Stop();
        musicSource.PlayOneShot(worldMusic, 0.5f);
    }

    public void PlayMenuMusic() {
        //second argument is priority level of sound (0-1)
        musicSource.Stop();
        musicSource.PlayOneShot(menuMusic, 0.5f);
    }

}
