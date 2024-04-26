using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodable : MonoBehaviour
{
    [SerializeField] private AudioClip explodeSFX;
    private SoundManager soundManager;

    private void Awake() {
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void OnDisable() {
        soundManager.PlaySoundEffectAtPoint(explodeSFX, transform.position, 0.7f);
    }
}
