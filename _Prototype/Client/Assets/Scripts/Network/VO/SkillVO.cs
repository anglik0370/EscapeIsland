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

    /// <summary>
    /// 이안,랜디, 앰버 생성자
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
    /// 사이먼 생성자
    /// </summary>
    /// <param name="skillType"></param>
    /// <param name="useSkillPlayerId"></param>
    /// <param name="itemId"></param>
    /// <param name="team"></param>
    /// <param name="skillName"></param>
    public SkillVO(CharacterType skillType, int useSkillPlayerId, int itemId, Team team, string skillName)
    {
        this.skillType = skillType;
        this.useSkillPlayerId = useSkillPlayerId;
        this.targetId = itemId;
        this.team = team;
        this.skillName = skillName;
    }
    
    /// <summary>
    /// 레이, 레옹 생성자
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
    /// 기온 생성자
    /// </summary>
    /// <param name="skillType"></param>
    /// <param name="useSkillPlayerId"></param>
    /// <param name="skillName"></param>
    /// <param name="point"></param>
    public SkillVO(CharacterType skillType, int useSkillPlayerId, string skillName, Team team, Vector2 point)
    {
        this.skillType = skillType;
        this.useSkillPlayerId = useSkillPlayerId;

        this.team = team;
        this.skillName = skillName;
        this.point = point;
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
