using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArsonSlot : ItemSlot
{
    public int id;
    public bool isArson = false;

    private void OnEnable()
    {
        SetItem(null);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetArson(bool on)
    {
        isArson = on;
        SetActive(on);
    }
}
