using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class BuildNavMeshSurface : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navSurface;

    private void Update() {
        //For testing: builds a new NavMesh when pressing "P"
        if(Input.GetKeyDown(KeyCode.P)) {
            //UpdateNavMesh();
        }
    }
    public void UpdateNavMesh() {
        navSurface.BuildNavMesh();
    }

}
