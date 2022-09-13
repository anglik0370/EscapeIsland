using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new BuffSO", menuName = "SO/BuffSO/ImmediateBuffSO")]
public class ImmediateBuffSO : BuffSO
{
    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new ImmediateBuff(this, obj);
    }
}
