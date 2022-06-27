using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffSO : ScriptableObject
{
    public int id;

    public float duration;
    public bool isDurationStacked;
    public bool isEffectStacked;
    public bool isBuffed;

    public abstract TimedBuff InitializeBuff(GameObject obj);
}
