using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Item", menuName = "SO/ItemSO")]
public class ItemSO : ScriptableObject
{
    public int itemId;
    public string itemName;
    public Sprite itemSprite;

    public override string ToString()
    {
        return itemName;
    }
}
