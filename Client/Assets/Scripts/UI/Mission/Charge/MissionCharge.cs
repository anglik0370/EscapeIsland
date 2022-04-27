using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionCharge : MonoBehaviour, IMission
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

    public bool IsMaxCharge => curOpenCharger.IsMaxCharging;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();

        guage = GetComponentInChildren<ChargeGaugeMObj>();
        batterySlot = GetComponentInChildren<MissionBatterySlot>();
    }

    public void Init()
    {
        guage.SetProgress(0, 0);
    }

    public void InitCurCharger()
    {
        curOpenCharger.Init();
    }
    
    public void SetCurCharger(ItemCharger charger)
    {
        curOpenCharger = charger;

        guage.SetProgress(curOpenCharger.MaxChargingTime, curOpenCharger.CurChargingTime);

        if(curOpenCharger.IsMaxCharging)
        {
            batterySlot.SetBatteryItem();
        }
        else if(curOpenCharger.IsCharging)
        {
            batterySlot.SetEmptyBetteryItem();
        }
        else
        {
            batterySlot.SetNullItem();
        }
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

        if(curOpenCharger.IsMaxCharging)
        {
            batterySlot.SetBatteryItem();
        }
    }
}
