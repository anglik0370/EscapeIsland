using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRefineryHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        RefineryVO vo = JsonUtility.FromJson<RefineryVO>(payload);
        NetworkManager.instance.SetStartRefinery(vo.refineryId, vo.itemSOId);
    }

}
