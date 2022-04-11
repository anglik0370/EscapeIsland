using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SabotageHandler : IMsgHandler<Sabotage>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);

        SabotageVO vo = JsonUtility.FromJson<SabotageVO>(payload);

        generic.SetSabotageData(vo);
    }
}
