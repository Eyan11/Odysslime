using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MechanismBase
{

    private Vector3 endPos;
    private Vector3 startPos;

    [Header("Settings")]
    [SerializeField] private float pauseTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float bufferDistance;
    [SerializeField] private PlatformMoveSlime moveSlimeScript;
    [SerializeField] private AudioClip movingPlatformSFX;
    public bool isActive = true;
    private float pauseCountdown = 0f;
    private bool isMovingToEnd = true;
    private Vector3 direction;
    private SoundManager soundManager;
    private AudioSource movingAudioSource;

    private void Awake() {
        pauseCountdown = pauseTime;
        //start position is currentposition
        startPos = transform.position;
        //end position is position of "End Position" child gameobject
        endPos = transform.GetChild(0).position;

        //find direction from start to end
        direction = endPos - startPos;

        // gets soundManager
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void Update() {
        pauseCountdown -= Time.deltaTime;
        
        // Prevents updatign the platform until enough time passed
        if (pauseCountdown > 0) { return; }
        // Prevents platform movement if inactive
        if (!isActive) { 
            if (movingAudioSource) {
                Destroy(movingAudioSource);
            }

            return; 
        }

        // Sound effect for moving
        if (movingAudioSource == null || !movingAudioSource.isPlaying) {
            movingAudioSource = soundManager.PlaySoundEffectOnObject(movingPlatformSFX, gameObject, 0.5f);
        }

        if(isMovingToEnd) {
            MoveToEnd();
        }
        else if(!isMovingToEnd) {
            MoveToStart();
        }
    }

    public void MoveToEnd() {

        //move to end position
        transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
        
        //move slimes with platform
        moveSlimeScript.UpdateAgentPosition(direction, moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, endPos) < bufferDistance) {
            isMovingToEnd = false;
            pauseCountdown = pauseTime;
        }
    }

    public void MoveToStart() {

        //move to start position
        transform.position = Vector3.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);

        //move slimes with platform
        moveSlimeScript.UpdateAgentPosition(-direction, moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, startPos) < bufferDistance) {
            isMovingToEnd = true;
            pauseCountdown = pauseTime;
        }
    }

    public override void Activate()
    {
        isActive = true;
    }

    public override void Deactivate()
    {
        isActive = false;
    }
}
