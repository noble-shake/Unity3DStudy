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
     * 구현 목표
     * 1. 자신의 위치로 부터 distance 범위내에 랜덤한 포인트를 찾고 그 위치로 이동 시키게 한다.
     * 2. 어디 위치를 클릭하면 그 위치로 이동하게, 장애물은 피하도록
     */

    NavMeshAgent agent;
    Vector3 destination;
    [SerializeField] float range = 10f;

    [SerializeField] bool ShowRange = false;
    [SerializeField] Color colorRange;

    [SerializeField]
    float maxWaitTime = 3f;
    float waitTimer = 3f;

    // Off Mesh Link 활용
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
        // agent.SetDestination(); -> Vector3 Input (call by value)  ~ Update 문에서 동작
        // Mesh Surface 밖으로 이동하라고 할 경우, 처리 방법에 대해서도 고려 해야 한다.
        
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
            offMeshEnd = linkData.endPos + (Vector3.up * agent.height * 0.5f);  // endpos 를 그대로 쓰면, 플레인 안에 파묻히는 지점을 갈 수 있음.

            // OffMesh에 의해 자동으로 동작하는 기본 함수들을 멈추고, 자신의 스크립트대로 이동하게함.
            agent.isStopped = true;

            speedJump = Vector3.Distance(offMeshStart, offMeshEnd) / agent.speed;
            maxHeightJump = (offMeshEnd - offMeshStart).y + jumpHeight; // 일직선 점프를 막고, 특정 위치까지 이상 올라간 다음 원하는 위치에 착지 시키도록 함.
            // 고저차가 똑같아도, 점프하는 연출을 낼 수 있음.

            setOffMesh = true;
        }

        //
        ratioJump += Time.deltaTime / speedJump; // 0 ~ 1 ratio

        Vector3 movePos = Vector3.Lerp(offMeshStart, offMeshEnd, ratioJump);

        movePos.y = offMeshStart.y + maxHeightJump * ratioJump + (- jumpHeight * Mathf.Pow(ratioJump, 2));

        transform.position = movePos;
        if (ratioJump > 1) // 도착
        {
            ratioJump = 0f;
            agent.isStopped = false;
            setOffMesh = false;
            agent.CompleteOffMeshLink();
        }
    }

    private bool isArrive() {
        //거리로 체크
        //float distance = Vector3.Distance(destination, transform.position);
        //if(distance <= 0.1f)
        //{ 

        //}

        // NAV MEsh 한테 물어보기
        // agent.remainingDistance  // 잘 안 쓰임.

        // 멈춰있는지 체크
        if (agent.velocity == Vector3.zero)
        {
            return true;
        }
        return false;
    }

    // 범위 내 메쉬 중  이동가능한 랜덤 위치를 선택해줍니다.
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
