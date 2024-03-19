using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class HungrySlimeAbilities : SlimeAbilities
{
    [Header("Settings")]
    [SerializeField] private Transform orientation;
    [SerializeField] private GameObject slimeModel;
    [SerializeField] private float radiusIncrease = 0.5f;
    public float slimeSize = 1;
    private float maxSlimeSize = 6;
    private float interactionDistance = 1.2f;
    private float normalSpeed;
    private int pushablesMask;
    private RaycastHit raycastHit;
    private SphereCollider sphereCollider;
    private NavMeshAgent navMeshAgent;
    private SlimeFollowerMovement slimeFollowerMovement;


    private void Awake() {
        pushablesMask = 1 << 9;
        pushablesMask |= pushablesMask << 8;

        sphereCollider = GetComponent<SphereCollider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        slimeFollowerMovement = GetComponent<SlimeFollowerMovement>();
        normalSpeed = slimeFollowerMovement.movementSpeed;
    }

    private void OnCollisionStay(Collision collision) {
        GameObject obj = collision.gameObject;
        // Checks if its a pushable block
        Pushable pushable = obj.GetComponent<Pushable>();
        if (!pushable) { return; }
        // Continues the pushable state if big enough
        float sizeDif = pushable.size - slimeSize;
        if (sizeDif > 0.01) { return; }
        pushable.ContinuePush();
        slimeFollowerMovement.movementSpeed = normalSpeed * (MathF.Abs(sizeDif) > 0.01 ? 1 : 0.5f);
    }

    private void OnCollisionExit(Collision collision) {
        GameObject obj = collision.gameObject;
        // Checks if its a pushable block
        Pushable pushable = obj.GetComponent<Pushable>();
        if (!pushable) { return; }
        // 'Ends' push
        pushable.EndPush();
        slimeFollowerMovement.movementSpeed = normalSpeed;
    }

    public override void UseAbility() {
        if (slimeSize < maxSlimeSize) {
            // Checks to see if there is an object in front
            if (!Physics.BoxCast(transform.position, Vector3.one * slimeSize * 0.5f, orientation.forward, out raycastHit, orientation.transform.rotation , slimeSize + interactionDistance)) {
                return;
            }

            // Checks if its a hungee slime
            GameObject gameObj = raycastHit.collider.gameObject;
            HungrySlimeAbilities hungrySlimeAbilities = gameObj.GetComponent<HungrySlimeAbilities>();

            if (!hungrySlimeAbilities) {
                return;
            }

            // Used for killing slime
            SlimeVitality slimeVitality = gameObj.GetComponent<SlimeVitality>();
            if (!slimeVitality) {
                Debug.LogError("Missing SlimeVitality component in " + gameObj.name);
            }
            slimeVitality.enabled = false;

            // Increases size
            slimeSize += radiusIncrease * 2;
            slimeModel.transform.localScale += Vector3.one * radiusIncrease * 2;
            sphereCollider.radius += radiusIncrease;
            navMeshAgent.baseOffset += radiusIncrease;
            navMeshAgent.radius += radiusIncrease;
        }
    }
}
