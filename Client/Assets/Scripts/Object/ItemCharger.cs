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
    }

    private IEnumerator Charge_Routine()
    {
        while (curChargingTime < maxChargingTime)
        {
            curChargingTime += Time.deltaTime;
            yield return null;
        }
    }
}
