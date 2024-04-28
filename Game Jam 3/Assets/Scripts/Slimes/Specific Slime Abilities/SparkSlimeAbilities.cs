using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SparkSlimeAbilities : SlimeAbilities
{
    [Header("Settings")]
    [SerializeField] private GameObject slimeModel;
    [SerializeField] private GameObject chargeVFX;
    [SerializeField] private AudioClip chargeSFX;
    [SerializeField] private float interactionDistance = 6.0f;
    [Header("Radius should be bigger than the slime itself")]
    [SerializeField] private float frontalInteractionRadius = 4.0f;
    private SlimeVitality slimeVitality;
    private SoundManager soundManager;
    private int movablesMask;
    private float nextCheckWait = 0.1f;
    private float nextCheckTime;
    private RaycastHit raycastHit;
    private UIManager UIScript;
    private Animator lightAnimator;

    void Awake()
    {
        // Creates bit mask for pushable objects
        movablesMask = 1 << 12;

        // Retrieves slime stuff
        slimeVitality = GetComponent<SlimeVitality>();
        GameObject lightBulb = slimeModel.transform.Find("LightBulb").Find("LightBulb").gameObject;
        lightAnimator = lightBulb.GetComponent<Animator>();
        // Retrieves other stuff
        UIScript = FindObjectOfType<UIManager>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void FixedUpdate() {
        // Checks to see if an object can be interacted with
        InteractionCheck();
    }

    private void InteractionCheck() {
        // Prevents a raycast hit check until enough time passes
        if (nextCheckTime > Time.fixedTime) return;
        nextCheckTime = Time.fixedTime + nextCheckWait;

        // Raycast check on pushable objects
        Physics.SphereCast(transform.position - slimeModel.transform.forward * frontalInteractionRadius, frontalInteractionRadius, slimeModel.transform.forward, out raycastHit, interactionDistance, movablesMask);

        // Only show prompt when possible to interact
        do {
            if (!raycastHit.collider) break;
            Battery battery = raycastHit.collider.gameObject.GetComponent<Battery>();
            if (battery.IsFullyCharged()) break;

            if (!lightAnimator.GetBool("CanUseAbility")) {
                lightAnimator.SetBool("CanUseAbility", true);
            }

            UIScript.DisplayPrompt("Press Q to charge battery!", 0.2f);
            return;
        } while (false);

        if (lightAnimator.GetBool("CanUseAbility")) {
            lightAnimator.SetBool("CanUseAbility", false);
        }
    }

    public override void UseAbility() {
        // Prevents interaction if not interacting with ANYTHING
        if (!raycastHit.collider) { return; }

        // Increase's the battery charge
        GameObject colliderObj = raycastHit.collider.gameObject;
        Battery battery = colliderObj.GetComponent<Battery>();
        if (battery.IsFullyCharged()) return; // Prevents from overcharging
        battery.IncreaseCharge();

        // Plays SFX and VFX
        soundManager.PlaySoundEffectAtPoint(chargeSFX, transform.position, 0.6f);
        GameObject chargeVFXClone = Instantiate(chargeVFX, transform.position, quaternion.identity);
        chargeVFXClone.transform.Find("VFXSpawn").transform.position = transform.position;
        chargeVFXClone.transform.Find("VFXFlash").transform.position = transform.position;
        chargeVFXClone.transform.Find("VFXExplosion").transform.position = colliderObj.transform.position;

        // Kills slime
        slimeVitality.enabled = false;
    }
}
