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
    private int day = 1;

    private float endTime = 0f;

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
        if(endTime > 0f)
        {
            endTime -= Time.deltaTime;
        }
    }

    public void TimeRefresh(int day, bool isLightTime)
    {
        this.day = day;

        if (!isLightTime)
        {
            darkTimeEvent.Occurred();
            endTime = 15f;
            PopupManager.instance.ClosePopup();
            NetworkManager.instance.StopOrPlay(true);
            dayAndSlotText.text = $"{day}번째 밤";
        }
        else
        {
            lightTimeEvent.Occurred();
            dayAndSlotText.text = $"{day}번째 낮";
        }
    }

    public bool EndOfVote()
    {
        return endTime <= 0f;
    }
}
