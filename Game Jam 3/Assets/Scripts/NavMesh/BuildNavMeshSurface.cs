using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class BuildNavMeshSurface : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navSurface;

    public void UpdateNavMesh() {
        Debug.Log("UpdateNavMesh called, but temporarily disabled");
        //navSurface.BuildNavMesh();
    }

}
