using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] GameObject objCursor;
    Camera camMain;

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
            
        }
    }
}
