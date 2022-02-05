using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeRefineryHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        NetworkManager.instance.SetTakeRefineryIngotItem(int.Parse(payload));
    }
}
