using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class RebuildNavMesh : MonoBehaviour
{
    [SerializeField] NavMeshSurface surface;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        surface.BuildNavMesh(); // 부하가 심각하므로
    }
}
