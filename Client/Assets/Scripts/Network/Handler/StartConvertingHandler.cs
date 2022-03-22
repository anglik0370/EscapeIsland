using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartConvertingHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        RefineryVO vo = JsonUtility.FromJson<RefineryVO>(payload);
        NetworkManager.instance.SetStartConvert(vo.refineryId, vo.itemSOId);
    }
}