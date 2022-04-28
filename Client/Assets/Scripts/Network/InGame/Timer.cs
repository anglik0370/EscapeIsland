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
    public static Timer Instance { get; private set; }

    private bool needSetTimer = false;
    private bool isInGameTimer = false;
    private bool isVoteTimer = false;
    private bool needSetTime = false;

    private TimerVO timerVO = null;
    private SetTimeVO setTimeVO = null;

    [Header("인게임 타이머관련")]
    private Sequence timerSequence;
    [SerializeField]
    private Text inGameTimerText;

    private const int HALF_DAY_MIN = 720;
    private int defaultHour = 6;
    private int min = 0;
    private int defaultMin = 0;
    private int destinationMin => min + perSec;
    private int perSec = 6;
    private float remainMin = 1f;

    private TimeSpan defaultTimeSpan;
    private TimeSpan curTimeSpan;

    [Header("투표 시간 타이머 관련")]
    private VotePopup voteTab;
    private float defaultVoteTimerMin = 180f;
    private float remainVoteTimerMin = 180f;

    [Header("긴급회의 타이머 관련")]
    public bool isEmergencyAble = true;
    private float defaultEmergencyCoolTime = 60f;
    private float remainEmergencyCoolTime = 0f;

    public static void SetTimer(TimerVO vo)
    {
        lock(Instance.lockObj)
        {
            Instance.timerVO = vo;
            Instance.needSetTimer = true;
        }
    }

    public static void SetTime(SetTimeVO vo)
    {
        lock(Instance.lockObj)
        {
            Instance.needSetTime = true;
            Instance.setTimeVO = vo;
        }
    }

    void Awake()
    {
        Instance = this;

        defaultTimeSpan = curTimeSpan = TimeSpan.FromHours(defaultHour);
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
        if(needSetTimer)
        {
            HandleTimer();
            needSetTimer = false;
        }
        if (needSetTime)
        {
            SetTime();
            needSetTime = false;
        }

        if (isInGameTimer)
        {
            InGameTimer();
        }

        if (isVoteTimer)
        {
            remainVoteTimerMin -= Time.deltaTime;

            voteTab.ChangeMiddleText(((int)remainVoteTimerMin).ToString());
        }

        if(!isEmergencyAble && !VoteManager.Instance.isVoteTime)
        {
            remainEmergencyCoolTime -= Time.deltaTime;
            if(remainEmergencyCoolTime <= 0f)
            {
                isEmergencyAble = true;
            }
        }
    }

    private void InGameTimer()
    {
        remainMin += Time.deltaTime;

        if (remainMin >= 1f)
        {
            remainMin -= 1f;

            timerSequence.Kill();
            timerSequence = DOTween.Sequence();
            timerSequence.Append(DOTween.To(() => min, x =>
            {
                min = x;
                curTimeSpan = defaultTimeSpan + TimeSpan.FromMinutes(min);
                inGameTimerText.text = curTimeSpan.ToString(@"hh\:mm");
            }, destinationMin, 1f).SetEase(Ease.Linear));
        }
    }

    private void InitTime()
    {
        isInGameTimer = false;
        isVoteTimer = false;

        timerSequence.Kill();
        timerSequence = DOTween.Sequence();

        min = defaultMin;
        remainMin = 1f;

        remainVoteTimerMin = defaultVoteTimerMin;
        remainEmergencyCoolTime = defaultEmergencyCoolTime;

        isEmergencyAble = true;
        remainEmergencyCoolTime = 0f;

        inGameTimerText.text = defaultTimeSpan.ToString(@"hh\:mm");
    }

    public void SetTime()
    {
        defaultVoteTimerMin = remainVoteTimerMin = setTimeVO.voteTime;
        perSec = (HALF_DAY_MIN / setTimeVO.inGameTime);
    }
    public void InitEmergencyCoolTime()
    {
        remainEmergencyCoolTime = defaultEmergencyCoolTime;
    }

    public float EmergencyFillCoolTime()
    {
        return remainEmergencyCoolTime / defaultEmergencyCoolTime;
    }

    public void HandleTimer()
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
            remainVoteTimerMin = defaultVoteTimerMin;
            VoteManager.Instance.EndVoteTime();
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
