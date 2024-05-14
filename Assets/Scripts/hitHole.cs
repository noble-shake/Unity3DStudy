using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitHole : MonoBehaviour
{
    SpriteRenderer spriteRender;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    public void SetSorting(int _sortNum)
    { 
        if(spriteRender == null) spriteRender = GetComponent<SpriteRenderer>();

        spriteRender.sortingOrder = _sortNum;
    }
}
