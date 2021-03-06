using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillHandler : IMsgHandler<Kill>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);

        generic.SetDieData(JsonUtility.FromJson<KillVO>(payload));
    }
}
