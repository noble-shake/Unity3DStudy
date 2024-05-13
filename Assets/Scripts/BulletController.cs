using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    float force;
    Vector3 dest;
    Rigidbody rigid;
    bool gravity = false;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Start()
    {

        Destroy(gameObject, 5.0f);    
    }


    void Update()
    {
        if (!gravity)
        {
            transform.position = Vector3.MoveTowards(transform.position, dest, force * Time.deltaTime);
        }

    }

    public void setDest(Vector3 _dest, float _force)
    {
        dest = _dest;
        force = _force;
        rigid.useGravity = false;
        gravity = false;
    }

    public void AddForce(float _force) {
        rigid.useGravity = true;
        rigid.AddForce(transform.rotation * Vector3.forward * _force, ForceMode.Impulse);
        gravity = true;

    }
}
