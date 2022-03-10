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
    private bool isNightStart = false;
    private int killStack = 2;
    public int KillStack { get { return killStack; } set { killStack = value; } }
    private float curTime = 0f;
    private float timeToNextStack = 20f;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        dayAndSlotText.text = $"{day}번째 낮";

        Init();
    }

    private void Update()
    {
        if(endTime > 0f)
        {
            endTime -= Time.deltaTime;
        }
    }

    public void KillStackTimer()
    {
        if (!NetworkManager.instance.IsKidnapper()) return;

        if (isNightStart)
        {
            curTime -= Time.deltaTime;

            if(curTime <= 0f)
            {
                killStack++;

                curTime = timeToNextStack;
            }
        }

        cooltimeImg.UpdateUI(curTime, timeToNextStack);
    }


    public bool KillAble()
    {
        return killStack >= 2;
    }

    public void TimeRefresh(int day, bool isLightTime)
    {
        this.day = day;

        if (!isLightTime)
        {
            darkTimeEvent.Occurred();
            isNightStart = true;
            
            dayAndSlotText.text = $"{day}번째 밤";
        }
        else
        {
            lightTimeEvent.Occurred();
            isNightStart = false;
            dayAndSlotText.text = $"{day}번째 낮";
        }
    }

    public bool EndOfVote()
    {
        return endTime <= 0f;
    }

    public void Init()
    {
        isNightStart = false;
        killStack = 0;
        curTime = timeToNextStack;
    }
}
