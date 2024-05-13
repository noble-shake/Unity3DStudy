using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunController : MonoBehaviour
{
    [SerializeField] GameObject fabBullet;
    [SerializeField] Transform trsMuzzle;

    Camera mainCam;
    [SerializeField] GameObject tpsCam;
    [SerializeField] GameObject detailCam;

    [SerializeField] float gunMaxDistance = 250.0f;
    [SerializeField] float gunPower = 100f;
    Vector3 bulletDest;

    [SerializeField] bool gravity;

    private void Start()
    {
        mainCam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        gunPointer();
        shoot();
        snipe();
    }

    private void snipe()
    {
        if (Input.GetKey(KeyCode.Mouse1) && !detailCam.gameObject.activeSelf)
        {
            detailCam.SetActive(true);
            tpsCam.SetActive(false);
        }
        else if (!Input.GetKey(KeyCode.Mouse1) && detailCam.activeSelf) 
        {
            detailCam.SetActive(false);
            tpsCam.SetActive(true);
        }
    }

    // Look aim point
    private void gunPointer() {
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, gunMaxDistance, LayerMask.GetMask("Ground")))
        {
            bulletDest = hit.point;
        }
        else
        {
            bulletDest = mainCam.transform.position + mainCam.transform.forward * gunMaxDistance;
        }

        transform.LookAt(bulletDest);
    }

    private void shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            createBullet();
        }
    }

    private void createBullet()
    {
        GameObject go = Instantiate(fabBullet, trsMuzzle.position, transform.rotation);
        BulletController goSc = go.GetComponent<BulletController>();

        if (!gravity)
        {
            goSc.setDest(bulletDest, gunPower);
        }
        else
        {
            goSc.AddForce(gunPower);
        }
    }

    
}
