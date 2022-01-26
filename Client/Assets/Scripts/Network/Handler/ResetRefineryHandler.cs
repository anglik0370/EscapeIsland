using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRefineryHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {

        NetworkManager.instance.SetResetRefinery(int.Parse(payload));
    }
}
