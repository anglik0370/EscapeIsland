using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TimerType
{
    IN_GAME = 0,
    IN_VOTE,
}

public class Timer : ISetAble
{
    private bool needTimerRefresh = false;

    private bool isInGameTimer = false;
    private bool isVoteTimer = false;

    private TimerVO timerVO = null;

    [Header("인게임 타이머관련")]
    private Sequence timerSequence;
    [SerializeField]
    private Text inGameTimerText;

    private int hour = 6;
    private int defaultHour = 6;
    private int min = 0;
    private int defualtMin = 0;
    private int destinationMin => min + 6;
    private float remainMin = 1f;

    private bool isSingleDigit = false;

    [Header("투표 시간 타이머 관련")]
    private float defaultTimerMin = 180f;


    public void SetTimer(TimerVO vo)
    {
        timerVO = vo;
        needTimerRefresh = true;
    }

    protected override void Start()
    {
        base.Start();
        EventManager.SubExitRoom(InitTime);
        EventManager.SubBackToRoom(InitTime);

        timerSequence = DOTween.Sequence();
    }

    void Update()
    {
        if(needTimerRefresh)
        {
            HandleTimer();
            needTimerRefresh = false;
        }

        if(isInGameTimer)
        {
            remainMin += Time.deltaTime;

            if(remainMin >= 1f)
            {
                remainMin -= 1f;
                if (destinationMin >= 61f)
                {
                    min -= 60;
                    hour++;

                    isSingleDigit = hour < 10;
                }
                timerSequence.Kill();
                timerSequence = DOTween.Sequence();

                timerSequence.Append(DOTween.To(() => min, x =>
                {
                    min = x;
                    if(min < 60f)
                    {
                        if (isSingleDigit)
                        {
                            inGameTimerText.text = min < 10 ? $"0{hour}:0{min}" : $"0{hour}:{min}";
                        }
                        else
                        {
                            inGameTimerText.text = min < 10 ? $"{hour}:0{min}" : $"{hour}:{min}";
                        }
                    }
                    else
                    {
                        inGameTimerText.text = hour + 1 < 10 ? $"0{hour + 1}:00" : $"{hour + 1}:00";
                    }
                }, destinationMin, 1f).SetEase(Ease.Linear));

            }
        }

        if(isVoteTimer)
        {

        }
    }

    private void InitTime()
    {
        hour = defaultHour;
        min = defualtMin;
        remainMin = 1f;
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
        isInGameTimer = start;
    }

    private void VoteTimer(bool start)
    {
        isVoteTimer = start;

        if(!start)
        {
            VoteManager.Instance.EndVoteTime();
        }
    }
}
