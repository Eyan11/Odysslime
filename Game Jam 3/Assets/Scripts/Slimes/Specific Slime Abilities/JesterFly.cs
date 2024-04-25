using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesterFly : MonoBehaviour
{
    [SerializeField] private float flySpeed;
    [SerializeField] private float despawnHeight;

    private void Update() {
        //move upwards
        transform.Translate(Vector3.up * flySpeed * Time.deltaTime);

        //if above dissapear height, despawn
        if(transform.position.y > despawnHeight)
            Destroy(gameObject);
    }

 }
