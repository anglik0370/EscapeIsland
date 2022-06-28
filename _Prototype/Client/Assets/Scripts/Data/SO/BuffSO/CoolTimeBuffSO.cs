using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new BuffSO", menuName = "SO/BuffSO/CoolTimeBuffSO")]
public class CoolTimeBuffSO : BuffSO
{
    public float magnification;

    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new TimedCoolBuff(this, obj);
    }
}
