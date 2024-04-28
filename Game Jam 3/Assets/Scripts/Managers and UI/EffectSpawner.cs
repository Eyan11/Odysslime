using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    public float delay;
    public float destroyTime = 1.0f;
    public float volume = 0.5f;
    public AudioClip SFX;
    public GameObject VFX;
    private float beginTime;
    public bool isConfigured = false;
    private bool fullyConfigured = false;
    private SoundManager soundManager;

    private void Awake() {
        soundManager = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!fullyConfigured && isConfigured) {
            beginTime = Time.fixedTime + delay;
            fullyConfigured = true;
        }

        if (!fullyConfigured) return; // Needs to be internally configured
        if (beginTime > Time.fixedTime) return; // Needs to have the timer be surpassed first

        if (VFX) {
            GameObject newVFX = Instantiate(VFX, transform.position, quaternion.identity);
            Destroy(newVFX, destroyTime);
        }

        if (SFX) {
            soundManager.PlaySoundEffectAtPoint(SFX, transform.position, volume);
        }

        Destroy(this);
    }
}
