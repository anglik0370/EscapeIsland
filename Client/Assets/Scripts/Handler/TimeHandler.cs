using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        timer = timeToNextSlot;
    }

    private void Update() 
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            if(isLightTime)
            {
                darkTimeEvent.Occurred();
            }
            else
            {
                lightTimeEvent.Occurred();
            }

            isLightTime = !isLightTime;

            timer = timeToNextSlot;
        }
    }
}
