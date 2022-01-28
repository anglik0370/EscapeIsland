using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeHandler : MonoBehaviour
{
    public static TimeHandler Instance{get; private set;}

    [SerializeField]
    private bool isLightTime = true;

    [SerializeField]
    private EventSO darkTimeEvent;
    [SerializeField]
    private EventSO lightTimeEvent;

    [SerializeField]
    private Text dayAndSlotText;

    [SerializeField]
    private int day = 1;

    private bool needRefresh = false;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        dayAndSlotText.text = $"{day}번째 낮";
    }

    private void Update() 
    {
        if(needRefresh)
        {
            if (!isLightTime)
            {
                darkTimeEvent.Occurred();
                dayAndSlotText.text = $"{day}번째 밤";
            }
            else
            {
                lightTimeEvent.Occurred();
                dayAndSlotText.text = $"{day}번째 낮";
            }
            needRefresh = false;
        }
    }

    public void TimeRefresh(int day, bool isLightTime)
    {
        this.day = day;
        this.isLightTime = isLightTime;
        needRefresh = true;
    }
}
