using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeHandler : MonoBehaviour
{
    public static TimeHandler Instance { get; private set; }

    private const string IN_GAME_TIMER_TEXT = "06:00";
    private const string IN_GAME_NIGHT_TEXT = "18:00";
    private const int TIME_CYCLE = 720;

    [SerializeField]
    private Text dayAndSlotText;
    [SerializeField]
    private Text inGameTimerText;

    [SerializeField]
    private int day = 1;
    private bool isLightTime = true;

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
        this.isLightTime = isLightTime;
        inGameTimerText.text = isLightTime ? IN_GAME_TIMER_TEXT : IN_GAME_NIGHT_TEXT;
    }

    public void Init()
    {
        curkillCoolTime = killCoolTime;
        isKillAble = false;
        day = 1;
        dayAndSlotText.text = $"{day}번째 낮";
        inGameTimerText.text = isLightTime ? IN_GAME_TIMER_TEXT : IN_GAME_NIGHT_TEXT;
    }

    public void InitKillCool()
    {
        isKillAble = false;
        curkillCoolTime = killCoolTime;
    }
}
