using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    Animator anim;
    [SerializeField] Transform LookatObject;
    [SerializeField, Range(0.01f, 1.0f)] float LookatWeight;

    [SerializeField, Range(0, 1f)] float distanceToGround; // 발목 모델링과 땅 사이의 거리
    bool modeChange = false;

    List<string> listDance = new List<string>();

    bool doDance;
    bool DoDance
    {
        set
        {
            doDance = true;
            if (value == true)
            {

                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    anim.SetLayerWeight(1, 0);
                }
            }
            else
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    anim.SetLayerWeight(1, 1);
                }
            }
        }
        get => doDance;
    }

    public class cDance 
    {
        public bool purchase = false;
        public string danceName = "";
    }

    private void stopDance()
    {
        DoDance = false;
        anim.Play("Blend Tree");
    }

    private void initAnimDance() {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;  // 현재 오브젝트에 등록된 애니메이션 컨트롤러 ( = 애니메이터 ) 안에 등록된 애니메이션 스테이트들을 가져온다.
        int count = clips.Length;
        for (int inum = 0; inum < count; ++inum)
        {
            if (clips[inum].name.Contains("Dance_"))
            {
                listDance.Add(clips[inum].name);
            }
        }
    }

    private void checkDance()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                anim.SetLayerWeight(1, 0);
            }
            // anim.Play(listDance[0]);
            anim.CrossFade(listDance[0], 0.05f);
            DoDance = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                anim.SetLayerWeight(1, 0);
            }
            // anim.Play(listDance[1]);
            anim.CrossFade(listDance[0], 0.05f);
            DoDance = true;
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (LookatObject != null)
        {
            anim.SetLookAtPosition(LookatObject.position);
            anim.SetLookAtWeight(LookatWeight);
        }

        
        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
        anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);


        Vector3 leftFootIKPos = anim.GetIKPosition(AvatarIKGoal.LeftFoot);
        Vector3 rightFootIKPos = anim.GetIKPosition(AvatarIKGoal.RightFoot);
        if (Physics.Raycast(leftFootIKPos + Vector3.up, Vector3.down, out RaycastHit leftHit, distanceToGround + 1f, LayerMask.GetMask("Ground")))
        {
            Vector3 leftFootPos = leftHit.point;
            leftFootPos.y += distanceToGround;

            Vector3 vecForward = Vector3.ProjectOnPlane(transform.forward, leftHit.normal);
            // Vector3 vecForward = transform.TransformDirection(Vector.forward);
            Vector3 vecUpward = leftHit.normal;

            anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPos);
            anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(vecForward, vecUpward));
        }
        if (Physics.Raycast(rightFootIKPos + Vector3.up, Vector3.down, out RaycastHit rightHit, distanceToGround + 1f, LayerMask.GetMask("Ground")))
        {
            Vector3 rightFootPos = rightHit.point;
            rightFootPos.y += distanceToGround;

            Vector3 vecForward = Vector3.ProjectOnPlane(transform.forward, rightHit.normal);
            Vector3 vecUpward = rightHit.normal;

            anim.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPos);
            anim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(vecForward, vecUpward));

        }

        
    }


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        anim = GetComponent<Animator>();
        initAnimDance();
    }


    void Update()
    {
        Move();
        checkAnim();
        checkDance();

    }

    private void checkAnim()
    {
        if (!DoDance && !modeChange && Input.GetKeyDown(KeyCode.LeftControl))
        {
            modeChange = true;
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                StartCoroutine(changeState(false));
            }
            else 
            {
                Cursor.lockState = CursorLockMode.Locked;
                StartCoroutine(changeState(true));
            }
        }
    }

    private void Move()
    {
        // anim.SetFloat("SpeedVertical", Input.GetAxisRaw("Vertical"));
        //anim.SetFloat("SpeedHorizontal", Input.GetAxisRaw("Horizontal"));
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        // blend tree animation
        anim.SetFloat("SpeedVertical", vertical);
        anim.SetFloat("SpeedHorizontal", horizontal);

        if (vertical != 0 || horizontal != 0)
        {
            stopDance();
        }

    }

    IEnumerator changeState(bool _upper) 
    {
        float time = 0f;
        if (_upper)
        {
            while (anim.GetLayerWeight(1) < 1) {
                time += Time.deltaTime * 5;
                if (time > 1f) {
                    time = 1;
                }
                
                // anim.SetLayerWeight(1, time);
                anim.SetLayerWeight(1, Mathf.Lerp(0, 1, time));

                yield return null;
            }
        }
        else
        {
            while (anim.GetLayerWeight(1) > 0)
            {
                time += Time.deltaTime * 5;
                if (time > 1f)
                {
                    time = 1;
                }

                // anim.SetLayerWeight(1, time);
                anim.SetLayerWeight(1, Mathf.Lerp(1, 0, time));

                yield return null;
            }
        }

        modeChange = false;
    }
}
