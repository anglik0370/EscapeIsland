using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionBatterySlot : ItemSlot
{
    private MissionBattery missionBattery;

    public Image Image => image;

    [SerializeField]
    private Sprite batterySprite;
    public Sprite BatterySprite => batterySprite;

    [SerializeField]
    private ItemSO emptyBatterySO;
    public ItemSO EmptyBatterySO => emptyBatterySO;

    [SerializeField]
    private ItemSO batterySO;
    public ItemSO BatterySO => batterySO;

    public new bool IsEmpty => image.sprite == null;
    public bool IsMaxCharge => missionBattery.IsMaxCharge;

    protected override void Awake()
    {
        missionBattery = GetComponentInParent<MissionBattery>();

        base.Awake();
    }

    public void SetBatteryItem()
    {
        item = batterySO;
    }

    public void StartCharging()
    {
        missionBattery.StartCharging();
    }

    public void InitCharger()
    {
        missionBattery.InitCharger();
    }
}
