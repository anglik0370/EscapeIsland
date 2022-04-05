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
    private int defaultMin = 0;
    private int destinationMin => min + 6;
    private float remainMin = 1f;

    private bool isSingleDigit = true;

    [Header("투표 시간 타이머 관련")]
    private VotePopup voteTab;
    private float defaultVoteTimerMin = 180f;
    private float remainVoteTimerMin = 180f;


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

        StartCoroutine(CoroutineHandler.Frame(() => voteTab = VoteManager.Instance.voteTab));
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
                    hour = hour > 23 ? 0 : hour;

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
                        if(hour + 1 > 23)
                        {
                            inGameTimerText.text = $"00:00";
                        }
                        else
                        {
                            inGameTimerText.text = hour + 1 < 10 ? $"0{hour + 1}:00" : $"{hour + 1}:00";
                        }
                    }
                }, destinationMin, 1f).SetEase(Ease.Linear));

            }
        }

        if(isVoteTimer)
        {
            remainVoteTimerMin -= Time.deltaTime;

            voteTab.ChangeMiddleText(((int)remainVoteTimerMin).ToString());
        }
    }

    private void InitTime()
    {
        hour = defaultHour;
        min = defaultMin;
        remainMin = 1f;

        remainVoteTimerMin = defaultVoteTimerMin;
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
        isVoteTimer = !start;

        if (!isVoteTimer)
        {
            VoteManager.Instance.EndVoteTime();
            remainVoteTimerMin = defaultVoteTimerMin;
        }
    }

    private void VoteTimer(bool start)
    {
        isVoteTimer = start;
        isInGameTimer = !start;

        if(!isInGameTimer)
        {
            
        }
    }
}
