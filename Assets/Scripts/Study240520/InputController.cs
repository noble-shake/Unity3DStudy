using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController instance;
    Camera camMain;
    UnitManager unitManager;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        camMain = Camera.main;
        unitManager = UnitManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        checkMouse();   
    }

    private void checkMouse()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        { 
            Ray ray = camMain.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit,100f, LayerMask.GetMask("Ground")))
            {
                unitManager.MoveALLAgent(hit.point);
            }
        }
    }
}
