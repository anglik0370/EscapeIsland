using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetConverterHandler : MonoBehaviour, IMsgHandler
{
    private Converter converter = null;
    private bool once = false;
    public void HandleMsg(string payload)
    {
        if(!once)
        {
            converter = NetworkManager.instance.FindSetDataScript<Converter>();
            once = true;
        }
        converter.SetResetConverter(int.Parse(payload));
    }
}
