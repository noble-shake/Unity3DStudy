using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControllerChar : MonoBehaviour
{
    CharacterController cController;
    [SerializeField] bool isGround;
    bool isJump;
    [SerializeField] float verticalVel;
    [SerializeField] float mouseSensivity;
    float moveSpeed = 5f;
    float jumpForce = 5f;
    float gravity = 9.81f;
    Vector3 moveDir;
    Vector3 rotateValue;

    Camera mainCam;

    //sliding
    [SerializeField] bool isSlope;
    [SerializeField] Vector3 slopeVel;

    // Start is called before the first frame update
    void Start()
    {
        cController = GetComponent<CharacterController>();
        mainCam = Camera.main;
    }

    private void OnDrawGizmos()
    {
        if (cController != null) {
            Gizmos.color = Color.red;
            Vector3 dist = transform.position - new Vector3(0, cController.height * 0.6f, 0f);
            Gizmos.DrawLine(transform.position, dist);
        }

    }

    // Update is called once per frame
    void Update()
    {
        checkGround();
        moving();
        jumping();
        checkGravity();

        rotating();
    }

    // from cinemachine, 시네머신이 어디를 보고 있는지, 데이터만 가져와서 캐릭터가 그 방향을 쳐다보게 하기.
    private void rotating()
    {
        transform.rotation = Quaternion.Euler(0, mainCam.transform.eulerAngles.y,0);
    }

    private void checkGround() {
        isGround = false;
        if (verticalVel < 0f)
        {
            isGround = Physics.Raycast(transform.position, Vector3.down, cController.height * 0.6f, LayerMask.GetMask("Ground"));
        }
    }

    private void moving() {
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        if (isSlope)
        {
            cController.Move(-slopeVel * Time.deltaTime);
        }
        else {
            cController.Move(transform.rotation * moveDir * moveSpeed * Time.deltaTime);  // X axis, Y axis only defined.        
        }


    }

    private void jumping() {
        if (isGround == false) return;

        if (Input.GetKeyDown(KeyCode.Space)) {
            isJump = true;
        }
    }

    private void checkGravity() {
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

        Vector3 moveVel = Vector3.zero;
        moveVel.y = verticalVel;
        cController.Move(moveVel * Time.deltaTime);

    }

    private void checkSlope() {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit Hit, cController.height)) {
            float angle = Vector3.Angle(Hit.normal, Vector3.up);

            if (angle > cController.slopeLimit)
            {
                isSlope = true;
            }
            else {
                isSlope = false;
            }

        }
    }

}
