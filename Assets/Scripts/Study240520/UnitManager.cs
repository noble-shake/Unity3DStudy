using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    Camera camMain;

    private void Start()
    {
        camMain = Camera.main;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { 
            Destroy(gameObject);
        }
    }

    private List<NPCNavMesh> listNPC = new List<NPCNavMesh>();

    public void AddAgent(NPCNavMesh _value)
    {
        listNPC.Exists((x) => x == _value);

        listNPC.Add(_value);        
    }

    public void RemoveAgent(NPCNavMesh _value)
    { 
        listNPC.Remove(_value);
    }

    public void MoveALLAgent(Vector3 _movePosition) {
        int count = listNPC.Count;

        for (int iNum = 0; iNum < count; ++iNum)
        {
            NPCNavMesh npc = listNPC[iNum];
            npc.SetDestination(_movePosition);
        }
    }

    public void ClearSelectAllUnit()
    {
        int count = listNPC.Count;
        for (int inum = 0; inum < count; inum++)
        {
            listNPC[inum].IsSelect = false;
        }
    }

    public void SelectUnit(Rect _rect)
    {
        int count = listNPC.Count;
        for (int inum = 0; inum < count; inum++)
        {
            NPCNavMesh unit = listNPC[inum];
            if (_rect.Contains(camMain.WorldToScreenPoint(unit.transform.position)) == true)
            {
                unit.IsSelect = true;
            }
        }
    }

}
