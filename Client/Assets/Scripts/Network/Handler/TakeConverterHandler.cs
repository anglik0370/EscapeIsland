using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeConverterHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        NetworkManager.instance.SetTakeConverterAfterItem(int.Parse(payload));
    }
}
