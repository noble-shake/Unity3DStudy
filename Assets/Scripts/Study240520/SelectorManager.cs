using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorManager : MonoBehaviour
{
    [SerializeField] RectTransform rectTrs;

    Rect selectorRect;
    Vector2 vecStart;
    Vector2 vecEnd;

    UnitManager unitManager;

    // Start is called before the first frame update
    void Start()
    {
        rectTrs.gameObject.SetActive(false);
        unitManager = UnitManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            unitManager.ClearSelectAllUnit();
            selectorRect = new Rect();
            vecStart = Input.mousePosition;
            rectTrs.gameObject.SetActive(true);
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            drawSelector();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            checkSelectUnit();
            rectTrs.gameObject.SetActive(false);
            vecStart = Vector2.zero;
            vecEnd= Vector2.zero;
        }
    }

    private void drawSelector()
    {
        // direction check. -> have to correct Center Point.
        vecEnd = Input.mousePosition;
        Vector2 center = (vecStart + vecEnd) * 0.5f;
        rectTrs.position = center;
        float sizeX = Mathf.Abs(vecStart.x - vecEnd.x);
        float sizeY = Mathf.Abs(vecStart.y - vecEnd.y);
        rectTrs.sizeDelta = new Vector2(sizeX, sizeY);
    }


    private void checkSelectUnit() {
        selectorRect.xMin = vecEnd.x < vecStart.x ? vecEnd.x : vecStart.x;
        selectorRect.yMin = vecEnd.y < vecStart.y ? vecEnd.y : vecStart.y;

        selectorRect.xMax = vecEnd.x < vecStart.x ? vecStart.x : vecEnd.x;
        selectorRect.yMax = vecEnd.y < vecStart.y ? vecStart.y : vecEnd.y;

        unitManager.SelectUnit(selectorRect);
    }

}
