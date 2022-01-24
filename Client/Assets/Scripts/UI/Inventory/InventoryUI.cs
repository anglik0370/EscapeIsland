using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryUI : MonoBehaviour
{
    public List<ItemSlot> slots = new List<ItemSlot>();

    private void Awake() 
    {
        slots = GetComponentsInChildren<ItemSlot>().ToList();
    }
}
