using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillVO
{
    public int targetId;

    public SkillVO(int targetId)
    {
        this.targetId = targetId;
    }
}
