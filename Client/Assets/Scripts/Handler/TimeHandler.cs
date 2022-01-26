using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeHandler : MonoBehaviour
{
    public static TimeHandler Instance{get; private set;}

    [SerializeField]
    private float timer; //현재 시간
    [SerializeField]
    private float timeToNextSlot; //낮 밤이 바뀌는 주기

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

    private bool inGame;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        timer = timeToNextSlot;

        dayAndSlotText.text = $"{day}번째 낮";
    }

    private void Update() 
    {
        if(inGame)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                if (isLightTime)
                {
                    darkTimeEvent.Occurred();
                    dayAndSlotText.text = $"{day}번째 밤";
                }
                else
                {
                    lightTimeEvent.Occurred();
                    day++;

                    dayAndSlotText.text = $"{day}번째 낮";
                }

                isLightTime = !isLightTime;

                timer = timeToNextSlot;
            }
        }
    }
    public void SetGame(bool on)
    {
        timer = timeToNextSlot;
        inGame = on;
    }
}
