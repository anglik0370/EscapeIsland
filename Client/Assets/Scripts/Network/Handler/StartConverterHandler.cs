using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartConverterHandler : MonoBehaviour,IMsgHandler
{
    private Converter converter = null;
    private bool once = false;

    public void HandleMsg(string payload)
    {
        if (!once)
        {
            converter = NetworkManager.instance.FindSetDataScript<Converter>();
            once = true;
        }

        RefineryVO vo = JsonUtility.FromJson<RefineryVO>(payload);
        converter.SetStartConvert(vo.refineryId, vo.itemSOId);
    }
}
