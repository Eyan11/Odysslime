using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class MagicSlimeAbilities : SlimeAbilities
{
    [Header("Settings")]
    [SerializeField] private Transform orientation;
    [SerializeField] private GameObject slimeModel;
    private float interactionDistance = 1.2f;
    private float normalSpeed;
    private int movablesMask;
    private SphereCollider sphereCollider;
    private NavMeshAgent navMeshAgent;
    private SlimeFollowerMovement slimeFollowerMovement;
    private SoundManager soundManager;

    private void Awake() {
        movablesMask = 1 << 9;
        movablesMask |= movablesMask << 8;

        sphereCollider = GetComponent<SphereCollider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        slimeFollowerMovement = GetComponent<SlimeFollowerMovement>();
        normalSpeed = slimeFollowerMovement.movementSpeed;
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void OnCollisionStay(Collision collision) {

    }

    public override void UseAbility() {

    }
}
