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
    private float maxChargingTime = 7f;
    [SerializeField]
    private float curChargingTime = 0f;

    [SerializeField]
    private bool isMaxCharge = false;
    public bool IsMaxCharge => isMaxCharge;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();

        guage = GetComponentInChildren<ChargeGaugeMObj>();
        batterySlot = GetComponentInChildren<MissionBatterySlot>();
    }

    public void Init()
    {
        guage.SetProgress(maxChargingTime, curChargingTime);
    }
    
    public void InitCharger()
    {
        isMaxCharge = false;

        curChargingTime = 0f;
        guage.SetProgress(maxChargingTime, curChargingTime);
    }
    
    public void StartCharging()
    {
        StartCoroutine(Charge_Routine());
    }

    private IEnumerator Charge_Routine()
    {
        //�����ϱ� ��

        while(curChargingTime < maxChargingTime)
        {
            curChargingTime += Time.deltaTime;
            guage.SetProgress(maxChargingTime, curChargingTime);
            yield return null;
        }

        //������ ������ ���� ���� �����ָ� �ȴ�
        batterySlot.SetBatteryItem();

        isMaxCharge = true;
    }
}
