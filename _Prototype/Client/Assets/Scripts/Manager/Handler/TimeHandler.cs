using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeHandler : MonoBehaviour
{
    public static TimeHandler Instance { get; private set; }

    [SerializeField]
    private Text dayAndSlotText;
    [SerializeField]
    private Text inGameTimerText;

    [SerializeField]
    private int day = 1;
    private bool isLightTime = true;

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
            Init();
            EventManager.OccurTimeChange(true);
        });
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
        this.isLightTime = isLightTime;
    }

    public void Init()
    {
        day = 1;
        dayAndSlotText.text = $"{day}번째 낮";
    }

    public string GetCurTime()
    {
        return $"{dayAndSlotText.text} {inGameTimerText.text}";
    }
}
