using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarHandler : IMsgHandler<Altar>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);

        generic.SetAltarData(JsonUtility.FromJson<AltarVO>(payload));
    }
}
