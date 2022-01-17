using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//시간대
public enum eSlot
{
    LightTime, //낮 시간대
    DarkTime //밤 시간대
}

public class TimeHandler : MonoBehaviour
{
    public static TimeHandler Instance{get; private set;}

    [SerializeField]
    private float timer; //현재 시간
    [SerializeField]
    private float timeToNextSlot; //낮 밤이 바뀌는 주기

    [SerializeField]
    private eSlot curSlot; //현재 시간대

    public event Action<eSlot> OnSlotChanged = slot => {};

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
            switch(curSlot)
            {
                case eSlot.LightTime:
                    curSlot = eSlot.DarkTime;
                    break;
                case eSlot.DarkTime:
                    curSlot = eSlot.LightTime;
                    break;
            }

            OnSlotChanged(curSlot);

            timer = timeToNextSlot;
        }
    }
}
