using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject soulVisuals;
    private Vector3 startPos;
    private float lerpPercent = 0f;
    private Transform targetSlime = null;

    public void MoveSoulToSlime(Transform currentSlime, Transform _targetSlime) {
        //get target and turn on visuals
        transform.position = currentSlime.position;
        startPos = currentSlime.position;
        targetSlime = _targetSlime;
        lerpPercent = 0f;
        soulVisuals.SetActive(true);
    }

    private void Update() {
        //if given a target
        if(targetSlime != null) {
            //move to it
            lerpPercent += speed * Time.deltaTime;
            //uses lerp for smooth movement
            transform.position = Vector3.Lerp(startPos, targetSlime.position, lerpPercent);

            //if soul is 95% of distance to target
            if(lerpPercent > 0.95) {
                    
                //dissapear and stop following
                targetSlime = null;
                soulVisuals.SetActive(false);
            }
        }
    }


}
