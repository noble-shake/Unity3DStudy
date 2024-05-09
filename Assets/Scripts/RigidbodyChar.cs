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
        // Automatic Center of Mass : �߽����� ��� ���� ���ΰ�? �ڵ��� �Ĺ� ����/ ���� ���� ���� ��. ���� �߽�
        // isKinematic �ڵ尡 �ƴ� �ܺ� ������ ������ ������
        // collistion detection -> prevent unity tunnerling, rotation�� ������ �ָ�.. �����δ� ȸ���� �ƴ϶� �̵���Ű�°��̶� �ε����� �ʰ� �հ� ��������
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

        rigid.velocity = transform.rotation * moveDir * moveSpeed; //2D �ʹ� �޸�, rotation�� ������� �Ѵ�.
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
        // �÷��� ���� �ƴ� ��.

    }
#endif

    private void checkGround()
    {
        isGround = false;
        if (verticalVel < 0f)  // jump ���߿��� ������
        {
            float height = capsuleCollider.height * 0.5f;
            float radius = capsuleCollider.radius;
            Vector3 checkPos = transform.position - new Vector3(0, height - radius);

            Collider[] colls = Physics.OverlapSphere(checkPos, radius + 0.01f, LayerMask.GetMask("Ground"));
            //overlap Collider���� �����ȿ��� ����

            if (colls.Length > 0)
            {
                isGround = true;
            }
        }

    }
}
