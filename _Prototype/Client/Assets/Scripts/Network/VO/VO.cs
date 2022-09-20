using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class VO
{
    public string ToJson() => JsonUtility.ToJson(this);
}
