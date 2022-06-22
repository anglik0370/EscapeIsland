using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionCharge : MonoBehaviour, IGetMission
{
    private CanvasGroup cvs;
    public CanvasGroup Cvs => cvs;

    private ChargeGaugeMObj guage;
    private MissionBatterySlot batterySlot;

    [SerializeField]
    private MissionType missionType;
    public MissionType MissionType => missionType;

    [SerializeField]
    private ItemCharger curOpenCharger;
    public ItemCharger CurOpenCharger => curOpenCharger;

    public bool IsMaxCharge => curOpenCharger.CurChargingTime >= curOpenCharger.MaxChargingTime;
    public bool IsCharging => !IsMaxCharge && curOpenCharger.CurChargingTime > 0;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();

        guage = GetComponentInChildren<ChargeGaugeMObj>();
        batterySlot = GetComponentInChildren<MissionBatterySlot>();
    }

    public void Open()
    {

    }

    public void Close()
    {
        guage.SetProgress(7, 0);

        
    }

    public void InitCurCharger()
    {
        curOpenCharger.Init();
    }
    
    public void SetCurCharger(ItemCharger charger)
    {
        curOpenCharger = charger;

        guage.SetProgress(curOpenCharger.MaxChargingTime, curOpenCharger.CurChargingTime); 
    }


    public void SetEmptyBattery()
    {
        if (curOpenCharger == null) return;

        curOpenCharger.StartCharging();
    }
  
    private void Update()
    {
        if (curOpenCharger == null) return;

        guage.SetProgress(curOpenCharger.MaxChargingTime, curOpenCharger.CurChargingTime);

        if(curOpenCharger.CurChargingTime >= curOpenCharger.MaxChargingTime)
        {
            batterySlot.SetBatteryItem();
        }
        else if(curOpenCharger.CurChargingTime <= 0)
        {
            batterySlot.SetNullItem();
        }
        else
        {
            batterySlot.SetEmptyBetteryItem();
        }
    }
}
