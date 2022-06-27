using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguishHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        Sabotage.SetExtinguishData();
    }
}
