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
    [SerializeField] private float boxCastHeight;
    [SerializeField] private float boxCastInterval;
    private float boxCastCountdown = 0;
    private Vector3 boxCastSize;
    private int slimeLayer;
    private bool isSquashingSlimes = false;
    public bool isActive = true;
    private float pauseCountdown = 0f;
    private bool isMovingToEnd = true;
    private Vector3 direction;
    private SoundManager soundManager;
    private AudioSource movingAudioSource;

    private void Awake() {
        pauseCountdown = pauseTime;
        boxCastCountdown = boxCastInterval;
        slimeLayer = 1 << LayerMask.NameToLayer("Slime");

        boxCastSize = transform.localScale;
        boxCastSize.y = boxCastHeight;

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
        boxCastCountdown -= Time.deltaTime;

        //every boxCastInterval seconds
        if(boxCastCountdown < 0) {
            boxCastCountdown = boxCastInterval;

            //check for collision with slime layer below platform
            if(Physics.BoxCast(transform.position, boxCastSize/2, -transform.up, Quaternion.identity, boxCastHeight/2, slimeLayer))
                isSquashingSlimes = true;
            else
                isSquashingSlimes = false;
        }
        
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
        //move to start if no slimes are in the way
        else if(!isMovingToEnd && !isSquashingSlimes) {
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

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position + (-transform.up * boxCastHeight/2), boxCastSize);
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
