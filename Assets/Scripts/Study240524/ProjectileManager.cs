using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] GameObject objCursor;
    Camera camMain;
    [SerializeField] Transform trsCannon;
    [SerializeField] Transform trsMuzzle;
    [SerializeField] GameObject BulletPrefab;

    [SerializeField] float time = 2f;
    [SerializeField] float ratio =0.5f;

    // Start is called before the first frame update
    void Start()
    {
        camMain = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        checkMouse();
    }

    private void checkMouse()
    {
        Ray ray = camMain.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
        {
            objCursor.SetActive(true);

            objCursor.transform.position = hit.point + Vector3.up * 0.0001f;

            Vector3 vo = calculateVelocity(hit.point);

            trsCannon.rotation = Quaternion.LookRotation(vo);

            if (Input.GetKeyDown(KeyCode.Mouse0)) 
            {
                GameObject go = Instantiate(BulletPrefab, trsMuzzle.position, Quaternion.identity);
                Rigidbody rb = go.GetComponent<Rigidbody>();
                rb.velocity = vo;
            }
        }
        else
        {
            objCursor.SetActive(false);
        }
    }

    private Vector3 calculateVelocity(Vector3 _target)
    {
        Vector3 distance = _target - trsCannon.position;

        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;  // float 으로 변경하고, 얼마나 긴지 (힘이 강한지)를 float으로 표현.

        float Vxz = Sxz / time;
        float Vy = Sy / time + ratio * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }
}
