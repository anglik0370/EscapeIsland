using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionBatterySlot : ItemSlot
{
    private MissionCharge missionBattery;

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

    public new bool IsEmpty => image.color == UtilClass.limpidityColor;
    public bool IsMaxCharge => missionBattery.IsMaxCharge;

    protected override void Awake()
    {
        missionBattery = GetComponentInParent<MissionCharge>();

        Image[] imgs = GetComponentsInChildren<Image>();

        image = imgs[itemImgDepth];

        SetNullItem();
    }

    //충전 완료됐을 때 콜되는 함수
    public void SetBatteryItem()
    {
        item = batterySO;

        image.color = UtilClass.opacityColor;
    }

    public void SetEmptyBetteryItem()
    {
        item = emptyBatterySO;

        image.color = UtilClass.opacityColor;
    }

    public void SetNullItem()
    {
        item = null;

        image.color = UtilClass.limpidityColor;
    }

    //충전 시작할 때 콜되는 함수
    public void StartCharging()
    {
        missionBattery.SetEmptyBattery();
    }

    //충전 끝난 배터리 가져갔을 때 콜되는 함수
    public void InitCurCharger()
    {
        missionBattery.InitCurCharger();
    }
}
