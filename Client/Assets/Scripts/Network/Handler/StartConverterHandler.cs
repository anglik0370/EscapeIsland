using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartConverterHandler : IMsgHandler<Converter>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);

        RefineryVO vo = JsonUtility.FromJson<RefineryVO>(payload);
        generic.SetStartConvert(vo.refineryId, vo.itemSOId);
    }
}
