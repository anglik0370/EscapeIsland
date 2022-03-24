using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeHandler : MonoBehaviour
{
    public static TimeHandler Instance{get; private set;}

    [SerializeField]
    private Text dayAndSlotText;

    [SerializeField]
    private CircleFillImage cooltimeImg;

    [SerializeField]
    private int day = 1;

    private float curkillCoolTime = 0f;
    public float CurKillCoolTime => curkillCoolTime;

    private float timeToNextStack = 20f;

    public bool isKillAble = false;

    private bool isGameStarted = false;

    private void Awake() 
    {
        if(Instance == null)
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
        if (NetworkManager.instance.User == null || (!NetworkManager.instance.User.isKidnapper || NetworkManager.instance.isVoteTime)) return;

        if (!isKillAble)
        {
            curkillCoolTime -= Time.deltaTime;

            if(curkillCoolTime <= 0f)
            {
                isKillAble = true;
            }
            cooltimeImg.UpdateUI(curkillCoolTime, timeToNextStack);
        }
    }


    public void TimeRefresh(int day, bool isLightTime)
    {
        this.day = day;

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

    public void Init()
    {
        curkillCoolTime = timeToNextStack;
        isKillAble = false;
        day = 1;
        dayAndSlotText.text = $"{day}번째 낮";
        cooltimeImg.UpdateUI(curkillCoolTime, timeToNextStack);
    }

    public void InitKillCool()
    {
        isKillAble = false;
        curkillCoolTime = timeToNextStack;
    }
}
