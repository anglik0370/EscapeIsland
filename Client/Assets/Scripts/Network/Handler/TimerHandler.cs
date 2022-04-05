using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimerHandler : IMsgHandler<Timer>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);
        TimerVO vo = JsonUtility.FromJson<TimerVO>(payload);
        generic.SetTimer(vo);
    }
}
