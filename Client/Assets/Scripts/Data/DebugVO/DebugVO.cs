using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 숫자키를 눌렀을 때 어떤 유형의 치트를 사용할지 결정합니다
/// </summary>
public enum CheatType
{
    None,
    GetItem,
    FillItem,
    KillPlayer,
}

[System.Serializable]
public class DebugVO
{
    public bool isMaster;
    public bool isKidnapper;
    public CheatType cheatType;

    public DebugVO(bool isMaster, bool isKidnapper, CheatType cheatType)
    {
        this.isMaster = isMaster;
        this.isKidnapper = isKidnapper;
        this.cheatType = cheatType;
    }
}
