using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolTimeBuffSO : BuffSO
{
    public float magnification;

    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new TimedCoolBuff(this, obj);
    }
}
