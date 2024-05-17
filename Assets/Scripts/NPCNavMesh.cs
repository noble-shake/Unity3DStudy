using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading;
#if UNITY_EDITOR
using UnityEditor;
#endif
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

    [SerializeField] bool ShowRange = false;
    [SerializeField] Color colorRange;

    [SerializeField]
    float maxWaitTime = 3f;
    float waitTimer = 3f;

    // Off Mesh Link Ȱ��
    [Header("Jump")]
    OffMeshLinkData linkData;
    Vector3 offMeshStart;
    Vector3 offMeshEnd;
    bool setOffMesh = false;
    float speedJump;
    float ratioJump;
    float maxHeightJump;
    [SerializeField] float jumpHeight = 5.5f;


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (ShowRange)
        {
            Handles.color = colorRange;
            Handles.DrawWireDisc(transform.position, Vector3.up, range);
        }
    }

#endif

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // agent.SetDestination(); -> Vector3 Input (call by value)  ~ Update ������ ����
        // Mesh Surface ������ �̵��϶�� �� ���, ó�� ����� ���ؼ��� ��� �ؾ� �Ѵ�.
        
    }

    void Start()
    {
        //destination = getRandomPoint();
        //agent.SetDestination(destination);
        UnitManager.Instance.AddAgent(this);
    }

    void Update()
    {
        //if (isArrive())
        //{
        //    destination = getRandomPoint();
        //    agent.SetDestination(destination);
        //}

        if (agent.isOnOffMeshLink == true) // if offmesh use, activated.
        {
            doOffMesh();
        }
    }

    private void doOffMesh()
    {
        // Set OffMesh Option
        if (setOffMesh == false)
        {
            linkData = agent.currentOffMeshLinkData;
            offMeshStart = transform.position;
            offMeshEnd = linkData.endPos + (Vector3.up * agent.height * 0.5f);  // endpos �� �״�� ����, �÷��� �ȿ� �Ĺ����� ������ �� �� ����.

            // OffMesh�� ���� �ڵ����� �����ϴ� �⺻ �Լ����� ���߰�, �ڽ��� ��ũ��Ʈ��� �̵��ϰ���.
            agent.isStopped = true;

            speedJump = Vector3.Distance(offMeshStart, offMeshEnd) / agent.speed;
            maxHeightJump = (offMeshEnd - offMeshStart).y + jumpHeight; // ������ ������ ����, Ư�� ��ġ���� �̻� �ö� ���� ���ϴ� ��ġ�� ���� ��Ű���� ��.
            // �������� �Ȱ��Ƶ�, �����ϴ� ������ �� �� ����.

            setOffMesh = true;
        }

        //
        ratioJump += Time.deltaTime / speedJump; // 0 ~ 1 ratio

        Vector3 movePos = Vector3.Lerp(offMeshStart, offMeshEnd, ratioJump);

        movePos.y = offMeshStart.y + maxHeightJump * ratioJump + (- jumpHeight * Mathf.Pow(ratioJump, 2));

        transform.position = movePos;
        if (ratioJump > 1) // ����
        {
            ratioJump = 0f;
            agent.isStopped = false;
            setOffMesh = false;
            agent.CompleteOffMeshLink();
        }
    }

    private bool isArrive() {
        //�Ÿ��� üũ
        //float distance = Vector3.Distance(destination, transform.position);
        //if(distance <= 0.1f)
        //{ 

        //}

        // NAV MEsh ���� �����
        // agent.remainingDistance  // �� �� ����.

        // �����ִ��� üũ
        if (agent.velocity == Vector3.zero)
        {
            return true;
        }
        return false;
    }

    // ���� �� �޽� ��  �̵������� ���� ��ġ�� �������ݴϴ�.
    private Vector3 getRandomPoint()
    {
        // RandominsideUnityCircle -> 2D Ranged Random value ~ 1.0
        // RandominsideUnitySphere -> 3D Ranged Random value ~ 1.0
        Vector3 randomPt = transform.position + Random.insideUnitSphere * range;
        if (NavMesh.SamplePosition(randomPt, out NavMeshHit hit, range, NavMesh.AllAreas))  // NavMesh.ALLAreas = -1
        {
            return hit.position;
        }

        return transform.position;
    }

    private bool waitTime()
    {
        if (waitTimer == 0.0f)
        {
            waitTimer = Random.Range(0.5f, maxWaitTime);
        }


        waitTimer += Time.deltaTime;
        if (waitTimer <= 0.0f)
        {
            return true;
        }

        return false;
    }

    public void SetDestination(Vector3 _pos)
    { 
        agent.SetDestination(_pos);
    }

}
