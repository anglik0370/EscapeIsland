using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeHandler : MonoBehaviour
{
    public static TimeHandler Instance { get; private set; }

    private const string IN_GAME_TIMER_TEXT = "06:00";
    private const int TIME_CYCLE = 720;

    [SerializeField]
    private Text dayAndSlotText;
    [SerializeField]
    private Text inGameTimerText;

    [SerializeField]
    private int day = 1;


    private Sequence timerSequence;
    private int hour = 6;
    private int min = 0;
    private int defaultHour = 6;
    private int defualtMin = 0;
    private int beforeMin = 0;

    private int destination = 0;
    private int count = 0;

    private float curkillCoolTime = 0f;
    public float CurKillCoolTime => curkillCoolTime;

    private float killCoolTime = 20f;
    public float KillCoolTime => killCoolTime;

    public bool isKillAble = false;
    private bool isSingleDigit = true;

    private bool isGameStarted = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        timerSequence = DOTween.Sequence();
    }

    private void Start()
    {
        Init();

        EventManager.SubGameOver(goc =>
        {
            Init();
            EventManager.OccurTimeChange(true);
        });

        EventManager.SubExitRoom(() =>
        {
            EventManager.OccurTimeChange(true);
        });

        EventManager.SubGameStart(p =>
        {
            isGameStarted = true;
        });
    }

    private void Update()
    {
        if (!isGameStarted) return;

        KillStackTimer();
    }

    public void KillStackTimer()
    {
        if (NetworkManager.instance.User == null || (!NetworkManager.instance.User.isKidnapper || VoteManager.Instance.isVoteTime)) return;

        if (!isKillAble)
        {
            curkillCoolTime -= Time.deltaTime;

            if (curkillCoolTime <= 0f)
            {
                isKillAble = true;
            }
        }
    }


    public void TimeRefresh(int day, bool isLightTime)
    {
        this.day = day;
        inGameTimerText.text = IN_GAME_TIMER_TEXT;

        if (!isLightTime)
        {
            EventManager.OccurTimeChange(false);
            dayAndSlotText.text = $"{day}번째 밤";
        }
        else
        {
            EventManager.OccurTimeChange(true);
            dayAndSlotText.text = $"{day}번째 낮";
        }
    }

    public void ChangeInGameTimeText(int time)
    {
        count = 0;
        min = beforeMin;

        destination = TIME_CYCLE - (time * 12);

        for (int i = 0; destination >= 60; i++)
        {
            count++;
            destination -= 60;
        }

        if(destination != 0 && destination <= 20)
        {
            hour = defaultHour;
            hour += count;

            min = defualtMin;
        }

        isSingleDigit = hour < 10;

        timerSequence.Kill();
        timerSequence = DOTween.Sequence();


        timerSequence.Append(DOTween.To(() => min, x =>
        {
            beforeMin = min = x;
            if(isSingleDigit)
            {
                inGameTimerText.text = min < 10 ? $"0{hour} : 0{min}" : $"0{hour} : {min}";
            }
            else
            {
                inGameTimerText.text = min < 10 ? $"{hour} : 0{min}" : $"{hour} : {min}";
            }
        }, destination > 0 ? destination : 59, 1f).SetEase(Ease.Linear));
    }

    public void Init()
    {
        curkillCoolTime = killCoolTime;
        isKillAble = false;
        day = 1;
        dayAndSlotText.text = $"{day}번째 낮";
        inGameTimerText.text = IN_GAME_TIMER_TEXT;
    }

    public void InitKillCool()
    {
        isKillAble = false;
        curkillCoolTime = killCoolTime;
    }
}
