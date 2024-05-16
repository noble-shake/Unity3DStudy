using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCNavMesh : MonoBehaviour
{
    /*
     * 구현 목표
     * 1. 자신의 위치로 부터 distance 범위내에 랜덤한 포인트를 찾고 그 위치로 이동 시키게 한다.
     * 2. 어디 위치를 클릭하면 그 위치로 이동하게, 장애물은 피하도록
     */

    NavMeshAgent agent;
    Vector3 destination;
    [SerializeField] float range = 10f;
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // agent.SetDestination(); -> Vector3 Input (call by value)  ~ Update 문에서 동작
        // Mesh Surface 밖으로 이동하라고 할 경우, 처리 방법에 대해서도 고려 해야 한다.
        
    }

    void Start()
    {
        destination = getRandomPoint();
        agent.SetDestination(destination);
    }

    void Update()
    {
        
    }

    // 범위 내 메쉬 중  이동가능한 랜덤 위치를 선택해줍니다.
    private Vector3 getRandomPoint()
    {
        // RandominsideUnityCircle -> 2D Ranged Random value ~ 1.0
        // RandominsideUnitySphere -> 3D Ranged Random value ~ 1.0
        Vector3 randomPt = transform.position + Random.insideUnitSphere * range;
        if (NavMesh.SamplePosition(randomPt, out NavMeshHit hit, range, NavMesh.AllAreas))  // NavMesh = -1
        {
            return hit.position;
        }

        return transform.position;
    }
}
