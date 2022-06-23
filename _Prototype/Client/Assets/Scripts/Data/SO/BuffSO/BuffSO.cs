using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new BuffSO", menuName = "SO/BuffSO")]
public abstract class BuffSO : ScriptableObject
{
    public int id;

    public float duration;
    public bool isDurationStacked;
    public bool isEffectStacked;

    public abstract TimedBuff InitializeBuff(GameObject obj);
}
