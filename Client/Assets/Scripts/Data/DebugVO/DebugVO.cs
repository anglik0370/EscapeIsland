using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ű�� ������ �� � ������ ġƮ�� ������� �����մϴ�
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
