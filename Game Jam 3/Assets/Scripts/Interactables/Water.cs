using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Lethal
{
    [SerializeField] private AudioClip waterSFX;
    private SoundManager soundManager;

    private void Awake() {
        soundManager = FindObjectOfType<SoundManager>();
    }

    protected override void OnTriggerEnter(Collider collider) {
        // Ice slime check
        GameObject gameObj = collider.gameObject;
        IceSlimeAbilities iceSlimeAbilities = gameObj.GetComponent<IceSlimeAbilities>();

        if (iceSlimeAbilities) {
            iceSlimeAbilities.GenerateIceCube();
        }

        soundManager.PlaySoundEffectAtPoint(waterSFX, gameObj.transform.position, 0.7f);

        base.OnTriggerEnter(collider);
    }
}
