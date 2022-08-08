using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillVO
{
    public CharacterType skillType;
    public int useSkillPlayerId;
    public int targetId;
    public string skillName;

    public Team team;
    public List<int> targetIdList;

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

    /// <summary>
    /// 이안,랜디 생성자
    /// </summary>
    /// <param name="skillType"></param>
    /// <param name="team"></param>
    public SkillVO(CharacterType skillType, int useSkillPlayerId, Team team,string skillName)
    {
        this.skillType = skillType;
        this.useSkillPlayerId = useSkillPlayerId;
        this.team = team;
        this.skillName = skillName;
    }
    
    /// <summary>
    /// 레이 생성자
    /// </summary>
    /// <param name="targetId"></param>
    public SkillVO(CharacterType skillType,int useSkillPlayerId, int targetId,string skillName)
    {
        this.skillType = skillType;
        this.useSkillPlayerId = useSkillPlayerId;
        this.targetId = targetId;
        this.skillName = skillName;
    }

    /// <summary>
    /// 조슈아, 체리 생성자
    /// </summary>
    /// <param name="team"></param>
    /// <param name="targetIdList"></param>
    public SkillVO(CharacterType skillType,int useSkillPlayerId, Team team, List<int> targetIdList, string skillName)
    {
        this.skillType = skillType;
        this.useSkillPlayerId = useSkillPlayerId;
        this.team = team;
        this.targetIdList = targetIdList;
        this.skillName = skillName;
    }
}
