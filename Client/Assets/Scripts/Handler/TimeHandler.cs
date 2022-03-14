using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeHandler : MonoBehaviour
{
    public static TimeHandler Instance{get; private set;}

    [SerializeField]
    private EventSO darkTimeEvent;
    [SerializeField]
    private EventSO lightTimeEvent;

    [SerializeField]
    private Text dayAndSlotText;

    [SerializeField]
    private CircleFillImage cooltimeImg;

    [SerializeField]
    private int day = 1;

    public float endTime = 0f;

    [Header("킬 관련")]
    private bool isNightTime = false;
    private float curTime = 0f;
    private float timeToNextStack = 20f;
    public bool isKillAble = false;

    private bool isGameStarted = false;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        dayAndSlotText.text = $"{day}번째 낮";

        Init();
    }

    private void Start()
    {
        EventManager.SubGameStart(p =>
        {
            isGameStarted = true;
            cooltimeImg.UpdateUI(curTime, timeToNextStack);
        });

        EventManager.SubBackToRoom(() =>
        {
            lightTimeEvent.Occurred();
            Init();
        });
    }

    private void Update()
    {
        if (!isGameStarted) return;

        if(endTime > 0f)
        {
            endTime -= Time.deltaTime;
        }

        KillStackTimer();
    }

    public void KillStackTimer()
    {
        if (!NetworkManager.instance.IsKidnapper() && NetworkManager.instance.isVoteTime) return;

        if (isNightTime && !isKillAble)
        {
            curTime -= Time.deltaTime;

            if(curTime <= 0f)
            {
                isKillAble = true;
                
                curTime = timeToNextStack;
            }
        }

        if(!isKillAble)
        {
            if(!EndOfVote())
            {
                cooltimeImg.UpdateUI(1, 1);
                cooltimeImg.SetColor(false);
            }
            else
            {
                cooltimeImg.UpdateUI(curTime, timeToNextStack);
                cooltimeImg.SetColor(true);
            }
        }
    }


    public void TimeRefresh(int day, bool isLightTime)
    {
        this.day = day;

        if (!isLightTime)
        {
            darkTimeEvent.Occurred();
            isNightTime = true;
            
            dayAndSlotText.text = $"{day}번째 밤";
        }
        else
        {
            lightTimeEvent.Occurred();
            isNightTime = false;
            dayAndSlotText.text = $"{day}번째 낮";
        }
    }

    public bool EndOfVote()
    {
        return endTime <= 0f;
    }

    public void Init()
    {
        isNightTime = false;
        curTime = timeToNextStack;
        isKillAble = false;
    }
}
