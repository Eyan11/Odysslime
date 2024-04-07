using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    private Vector3 endPos;
    private Vector3 startPos;

    [Header("Settings")]
    [SerializeField] private float pauseTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float bufferDistance;
    [SerializeField] private PlatformMoveSlime moveSlimeScript;
    private float pauseCountdown = 0f;
    private bool isMovingToEnd = true;
    private Vector3 direction;

    private void Awake() {
        pauseCountdown = pauseTime;
        //start position is currentposition
        startPos = transform.position;
        //end position is position of "End Position" child gameobject
        endPos = transform.GetChild(0).position;

        //find direction from start to end
        direction = endPos - startPos;
    }

    private void Update() {
        pauseCountdown -= Time.deltaTime;
        
        if(isMovingToEnd && pauseCountdown < 0) {
            MoveToEnd();
        }
        else if(!isMovingToEnd && pauseCountdown < 0) {
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


}
