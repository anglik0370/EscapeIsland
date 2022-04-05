using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimerType
{
    IN_GAME = 0,
    IN_VOTE,
}

public class Timer : ISetAble
{
    private bool needTimerRefresh = false;

    private TimerVO timerVO = null;

    public void SetTimer(TimerVO vo)
    {
        timerVO = vo;
        needTimerRefresh = true;
    }

    void Update()
    {
        if(needTimerRefresh)
        {
            HandleTimer();
            needTimerRefresh = false;
        }
    }

    private void HandleTimer()
    {
        TimerType type = (TimerType)Enum.Parse(typeof(TimerType),timerVO.type);

        switch (type)
        {
            case TimerType.IN_GAME:
                InGameTimer(timerVO.isStart);
                break;
            case TimerType.IN_VOTE:
                VoteTimer(timerVO.isStart);
                break;
            default:
                Debug.LogError("없는 타입입니다.");
                break;
        }
    }

    private void InGameTimer(bool start)
    {

    }

    private void VoteTimer(bool start)
    {
        //start false일때 vote_time_end 핸들러에서 해주던거 해주기
    }
}
