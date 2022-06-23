using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new BuffSO", menuName = "SO/BuffSO/SpeedBuff")]
public class SpeedBuffSO : BuffSO
{
    public float magnification;

    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new TimedSpeedBuff(this, obj);
    }
}
