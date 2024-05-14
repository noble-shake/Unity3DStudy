using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Experimental.AI;

public class RigidbodyChar : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] float gravity = 9.81f;
    [SerializeField] float mouseSensivity = 5f;
    [SerializeField] float verticalVel = 0f;
    [SerializeField] bool isGround = false;
    [SerializeField] bool isJump = false;
    [SerializeField] Vector3 moveDir;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    Vector3 rotateValue;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        // Automatic Center of Mass : 중심점을 어디에 잡을 것인가? 자동차 후방 엔진/ 전방 엔진 차이 등. 무게 중심
        // isKinematic 코드가 아닌 외부 물리도 적용할 것인지
        // collistion detection -> prevent unity tunnerling, rotation을 빠르게 주면.. 실제로는 회전이 아니라 이동시키는것이라 부딪히지 않고 뚫고 나가버림
        // continous << continous dynamic,   performance descrete better
        // Mesh Collider VERY HEAVY
    }

    void Start()
    {

    }

    void Update()
    {
        checkGround();
        Moving();
        Jump();
        checkGravity();
        rotating();
    }

    private void rotating()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensivity * Time.deltaTime;

        rotateValue.x -= mouseY;
        rotateValue.y += mouseX;

        rotateValue.x = Mathf.Clamp(rotateValue.x, -45f, 45f);
        rotateValue.y = Mathf.Clamp(rotateValue.y, 0f, 60f);

        // Character, Camera
        transform.rotation = Quaternion.Euler(0f, rotateValue.y, 0f);
        Camera.main.transform.rotation = Quaternion.Euler(rotateValue.x, rotateValue.y, 0f);

    }

    private void Moving()
    {
        moveDir.z = Input.GetAxisRaw("Vertical");
        moveDir.x = Input.GetAxisRaw("Horizontal");

        rigid.velocity = transform.rotation * moveDir * moveSpeed; //2D 와는 달리, rotation을 곱해줘야 한다.
    }

    private void Jump()
    {
        if (!isGround) return;

        if (Input.GetKeyDown(KeyCode.Space) == true)
        { 
            isJump = true;
        }
    }

    private void checkGravity()
    {
        if (isGround == true)
        {
            verticalVel = 0f;
        }

        if (isJump == true)
        {
            isJump = false;
            verticalVel = jumpForce;
        }
        else {
            verticalVel -= gravity * Time.deltaTime;
        }

        Vector3 curVel = rigid.velocity;
        curVel.y = verticalVel;
        rigid.velocity = curVel;
    }

#if UNITY_EDITOR
    [SerializeField] bool Show;
    [SerializeField] Color rangeColor;

    private void OnDrawGizmos()
    {
        if (Show == true && capsuleCollider != null)
        {
            float height = capsuleCollider.height * 0.5f;
            float radius = capsuleCollider.radius;
            Vector3 checkPos = transform.position + new Vector3(0, height - radius);
            Gizmos.DrawWireSphere(checkPos, radius + 0.01f);
            Gizmos.color = Color.red;

            Handles.color = rangeColor;
            Handles.DrawWireDisc(checkPos, Vector3.up, radius + 1);


        }
        // 플레이 중이 아닐 때.

    }
#endif

    private void checkGround()
    {
        isGround = false;
        if (verticalVel < 0f)  // jump 도중에는 막아줌
        {
            float height = capsuleCollider.height * 0.5f;
            float radius = capsuleCollider.radius;
            Vector3 checkPos = transform.position - new Vector3(0, height - radius);

            Collider[] colls = Physics.OverlapSphere(checkPos, radius + 0.01f, LayerMask.GetMask("Ground"));
            //overlap Collider들을 범위안에서 실행

            if (colls.Length > 0)
            {
                isGround = true;
            }
        }

    }
}
