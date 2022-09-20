using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillVO : VO
{
    public CharacterType skillType;
    public int useSkillPlayerId;
    public int targetId;
    public string skillName;

    public Vector2 point;
    public int itemId;

    public Team team;
    public List<int> targetIdList;
    public List<int> itemIdList;

    /// <summary>
    /// 기본 생성자
    /// </summary>
    /// <param name="type"></param>
    public SkillVO(CharacterType type,int useSkillPlayerId,string skillName)
    {
        this.skillType = type;
        this.useSkillPlayerId = useSkillPlayerId;

        this.skillName = skillName;
    }

    public SkillVO SetTargetId(int targetId)
    {
        this.targetId = targetId;

        return this;
    }

    public SkillVO SetPoint(Vector2 point)
    {
        this.point = point;

        return this;
    }

    public SkillVO SetItemId(int itemId)
    {
        this.itemId = itemId;

        return this;
    }

    public SkillVO SetTeam(Team team)
    {
        this.team = team;

        return this;
    }

    public SkillVO SetTargetIdList(List<int> targetIdList)
    {
        this.targetIdList = targetIdList;

        return this;
    }

    public SkillVO SetItemIdList(List<int> itemIdList)
    {
        this.itemIdList = itemIdList;

        return this;
    }
}
