using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCharger : MonoBehaviour
{
    [SerializeField]
    private int id;
    public int Id => id;

    [SerializeField]
    private bool isCharging;
    [SerializeField]
    private bool isMaxCharging;

    public bool IsCharging => isCharging;
    public bool IsMaxCharging => isMaxCharging;

    [SerializeField]
    private float maxChargingTime = 7f;
    [SerializeField]
    private float curChargingTime = 0f;

    public float MaxChargingTime => maxChargingTime;
    public float CurChargingTime => curChargingTime;

    public void StartCharging()
    {
        StartCoroutine(Charge_Routine());
    }

    public void Init()
    {
        curChargingTime = 0f;

        isCharging = false;
        isMaxCharging = false;
    }

    private IEnumerator Charge_Routine()
    {
        //�����ϱ� ��
        isCharging = true;

        while (curChargingTime < maxChargingTime)
        {
            curChargingTime += Time.deltaTime;
            yield return null;
        }

        //������ ������ ���� ���� �����ָ� ��
        isCharging = false;
        isMaxCharging = true;
    }
}
