using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListener : MonoBehaviour
{
    private void Start() 
    {
        TimeHandler.Instance.OnSlotChanged += slot =>
        {
            print(slot.ToString());
        };
    }
}
