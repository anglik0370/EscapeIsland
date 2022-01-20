using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryUI : MonoBehaviour
{
    public List<InventorySlotUI> slots = new List<InventorySlotUI>();

    private void Awake() 
    {
        slots = GetComponentsInChildren<InventorySlotUI>().ToList();
    }
}
