using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    [SerializeField] float speedRound = 5f;
    [SerializeField] Transform trsTarget;

    void Start()
    {
        
    }

    void Update()
    {
        moveAround();
    }

    private void moveAround()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            direction = Vector3.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction = Vector3.down;
        }

        transform.RotateAround(trsTarget.position, direction, speedRound * Time.deltaTime);
    }
}
