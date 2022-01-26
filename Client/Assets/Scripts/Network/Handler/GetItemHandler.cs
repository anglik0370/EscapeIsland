using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        int idx = int.Parse(payload);
        NetworkManager.instance.SetItemDisable(idx);
    }
}
