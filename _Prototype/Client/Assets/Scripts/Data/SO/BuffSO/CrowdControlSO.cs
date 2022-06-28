using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new BuffSO", menuName = "SO/BuffSO/CrowdControl")]
public class CrowdControlSO : BuffSO
{
    public bool isRestrict;

    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new CrowdControl(this, obj);
    }
}
