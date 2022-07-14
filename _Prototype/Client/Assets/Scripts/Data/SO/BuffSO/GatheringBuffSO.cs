using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new BuffSO", menuName = "SO/BuffSO/GatheringBuff")]
public class GatheringBuffSO : BuffSO
{
    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new GatheringBuff(this, obj);
    }
}
