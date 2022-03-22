using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetConverterHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {

        NetworkManager.instance.SetResetConverter(int.Parse(payload));
    }
}
