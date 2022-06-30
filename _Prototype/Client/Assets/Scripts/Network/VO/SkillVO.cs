using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillVO
{
    public SkillType skillType;
    public int targetId;

    public Team team;
    public List<int> targetIdList;

    /// <summary>
    /// �⺻ ������
    /// </summary>
    /// <param name="type"></param>
    public SkillVO(SkillType type)
    {
        this.skillType = type;
    }

    /// <summary>
    /// �̾�,���� ������
    /// </summary>
    /// <param name="skillType"></param>
    /// <param name="team"></param>
    public SkillVO(SkillType skillType,Team team)
    {
        this.skillType = skillType;
        this.team = team;
    }
    
    /// <summary>
    /// ���� ������
    /// </summary>
    /// <param name="targetId"></param>
    public SkillVO(SkillType skillType, int targetId)
    {
        this.skillType = skillType;
        this.targetId = targetId;
    }

    /// <summary>
    /// ������, ü�� ������
    /// </summary>
    /// <param name="team"></param>
    /// <param name="targetIdList"></param>
    public SkillVO(SkillType skillType, Team team, List<int> targetIdList)
    {
        this.skillType = skillType;
        this.team = team;
        this.targetIdList = targetIdList;
    }
}
