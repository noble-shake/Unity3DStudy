using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCNavMesh : MonoBehaviour
{
    /*
     * ���� ��ǥ
     * 1. �ڽ��� ��ġ�� ���� distance �������� ������ ����Ʈ�� ã�� �� ��ġ�� �̵� ��Ű�� �Ѵ�.
     * 2. ��� ��ġ�� Ŭ���ϸ� �� ��ġ�� �̵��ϰ�, ��ֹ��� ���ϵ���
     */

    NavMeshAgent agent;
    Vector3 destination;
    [SerializeField] float range = 10f;
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // agent.SetDestination(); -> Vector3 Input (call by value)  ~ Update ������ ����
        // Mesh Surface ������ �̵��϶�� �� ���, ó�� ����� ���ؼ��� ��� �ؾ� �Ѵ�.
        
    }

    void Start()
    {
        destination = getRandomPoint();
        agent.SetDestination(destination);
    }

    void Update()
    {
        
    }

    // ���� �� �޽� ��  �̵������� ���� ��ġ�� �������ݴϴ�.
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
