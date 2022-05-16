using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArsonSlot : ItemSlot
{
    public int id;

    private void OnEnable()
    {
        SetItem(null);
    }
}
