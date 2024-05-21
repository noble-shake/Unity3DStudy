using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    Animator anim;
    [SerializeField] Transform LookatObject;
    [SerializeField, Range(0.01f, 1.0f)] float LookatWeight;

    private void OnAnimatorIK(int layerIndex)
    {
        if (LookatObject != null)
        {
            anim.SetLookAtPosition(LookatObject.position);
            anim.SetLookAtWeight(LookatWeight);
        }
    }


    void Start()
    {
        anim = GetComponent<Animator>();        
    }


    void Update()
    {
        Move();
    }

    private void Move()
    {
        // anim.SetFloat("SpeedVertical", Input.GetAxisRaw("Vertical"));
        //anim.SetFloat("SpeedHorizontal", Input.GetAxisRaw("Horizontal"));


        // blend tree animation
        anim.SetFloat("SpeedVertical", Input.GetAxis("Vertical"));
        anim.SetFloat("SpeedHorizontal", Input.GetAxis("Horizontal"));
    }
}
