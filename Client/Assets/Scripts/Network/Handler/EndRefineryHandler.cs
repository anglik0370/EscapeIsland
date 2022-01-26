using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRefineryHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        NetworkManager.instance.SetEndRefinery(int.Parse(payload));
    }
}
