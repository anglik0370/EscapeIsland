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

    [Header("킬 관련")]
    private bool isNightTime = false;
    private float curkillCoolTime = 0f;
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
        dayAndSlotText.text = $"{day}번째 낮";

        EventManager.SubGameOver(goc =>
        {
            Init();
        });

        EventManager.SubGameStart(p =>
        {
            isGameStarted = true;
            EventManager.OccurTimeChange(true);
        });
    }

    private void Update()
    {
        if (!isGameStarted) return;

        KillStackTimer();
    }

    public void KillStackTimer()
    {
        if (!NetworkManager.instance.IsKidnapper() && NetworkManager.instance.isVoteTime) return;

        if (isNightTime && !isKillAble)
        {
            curkillCoolTime -= Time.deltaTime;

            if(curkillCoolTime <= 0f)
            {
                isKillAble = true;
                
                curkillCoolTime = timeToNextStack;
            }
        }

        if(!isKillAble)
        {
            cooltimeImg.UpdateUI(curkillCoolTime, timeToNextStack);
        }
    }


    public void TimeRefresh(int day, bool isLightTime)
    {
        this.day = day;

        if (!isLightTime)
        {
            EventManager.OccurTimeChange(false);
            isNightTime = true;
            
            dayAndSlotText.text = $"{day}번째 밤";
        }
        else
        {
            EventManager.OccurTimeChange(true);
            isNightTime = false;
            dayAndSlotText.text = $"{day}번째 낮";
        }
    }

    public void Init()
    {
        isNightTime = false;
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
