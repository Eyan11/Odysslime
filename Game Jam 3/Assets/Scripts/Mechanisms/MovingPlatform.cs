using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    private Vector3 endPos;
    private Vector3 startPos;
    private Vector3 direction;
    private float lerpPercent;

    [Header("Settings")]
    [SerializeField] private float pauseTime;
    [SerializeField] private float moveSpeed;
    private float pauseCountdown = 0f;
    private bool isMovingToEnd = true;

    private void Awake() {
        pauseCountdown = pauseTime;
        //start position is currentposition
        startPos = transform.position;
        //end position is position of "End Position" child gameobject
        endPos = transform.GetChild(0).position;
    }

    private void Update() {
        pauseCountdown -= Time.deltaTime;
        
        if(isMovingToEnd && pauseCountdown < 0 && lerpPercent < 1) {
            MoveToEnd();
        }
        else if(!isMovingToEnd && pauseCountdown < 0 && lerpPercent > 0) {
            MoveToStart();
        }
    }

    public void MoveToEnd() {

        //move to end position
        transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);

        if(lerpPercent == 1) {
            isMovingToEnd = false;
            pauseCountdown = pauseTime;
        }
    }

    public void MoveToStart() {

        //move to start position
        transform.position = Vector3.MoveTowards(startPos, startPos, moveSpeed * Time.deltaTime);

        if(lerpPercent == 0) {
            isMovingToEnd = true;
            pauseCountdown = pauseTime;
        }
    }


}
