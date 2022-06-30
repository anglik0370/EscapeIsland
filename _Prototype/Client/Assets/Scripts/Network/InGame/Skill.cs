using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ISetAble
{
    public static Skill Instance { get; private set; }

    private bool needSkillRefresh = false;

    private SkillVO skillData;

    [SerializeField] private const int JOSHUA_BUFF_ID = 3;
    [SerializeField] private const int RAI_BUFF_ID = 4;

    [SerializeField] private const int CHERRY_ENEMY_TEAM_DEBUFF_ID = 111;
    [SerializeField] private const int CHERRY_ENEMY_TEAM_DEBUFF_ID2 = 112;
    [SerializeField] private const int CHERRY_SAME_TEAM_BUFF_ID = 113;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(needSkillRefresh)
        {
            SkillRefresh();
            needSkillRefresh = false;
        }
    }
    public static void SetSkill(SkillVO vo)
    {
        lock(Instance.lockObj)
        {
            Instance.needSkillRefresh = true;
            Instance.skillData = vo;
        }
    }

    private void SkillRefresh()
    {
        switch (skillData.skillType)
        {
            case SkillType.Amber:
                break;
            case SkillType.Cherry:
                CherrySkill();
                break;
            case SkillType.IAN:
                RemoveAllDebuff();
                break;
            case SkillType.Joshua:
                JoshuaSkill();
                break;
            case SkillType.Rai:
                RaiSkill();
                break;
            case SkillType.Randy:
                DissRap();
                break;
            case SkillType.Sarsu:
                break;
            case SkillType.Wonsong:
                break;
            case SkillType.Ander:
                break;
            default:
                break;
        }
    }

    private void DissRap()
    {
        if(!user.CurTeam.Equals(skillData.team))
        {
            MissionPanel.Instance.CloseGetMissionPanel();
        }

        CreateSkillLog();
    }

    private void RemoveAllDebuff()
    {
        //trap 처리
        {
            List<Trap> trapList = Sabotage.Instance.GetTrapList();

            foreach (Trap trap in trapList)
            {
                if (trap.enterPlayerId.Equals(user.socketId))
                {
                    trap.Init();
                    break;
                }
            }
        }

        if(user.CurTeam.Equals(skillData.team))
        {
            user.BuffHandler.RemoveAllDebuff();
        }

        CreateSkillLog();
    }

    private void CherrySkill()
    {
        print("cherrySKill");

        if (skillData.targetIdList.Count > 0)
        {
            if (skillData.targetIdList.Contains(user.socketId))
            {
                if (user.CurTeam.Equals(skillData.team))
                {
                    user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(CHERRY_SAME_TEAM_BUFF_ID).InitializeBuff(user.gameObject));
                }
                else
                {
                    user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(CHERRY_ENEMY_TEAM_DEBUFF_ID).InitializeBuff(user.gameObject));
                    user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(CHERRY_ENEMY_TEAM_DEBUFF_ID2).InitializeBuff(user.gameObject));
                }
            }

            foreach (Player p in NetworkManager.instance.GetPlayerList())
            {
                if (!skillData.targetIdList.Contains(p.socketId)) continue;
                if (p.CurTeam.Equals(skillData.team))
                {
                    p.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(CHERRY_SAME_TEAM_BUFF_ID).InitializeBuff(p.gameObject));
                }
                else
                {
                    p.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(CHERRY_ENEMY_TEAM_DEBUFF_ID).InitializeBuff(p.gameObject));
                    p.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(CHERRY_ENEMY_TEAM_DEBUFF_ID2).InitializeBuff(p.gameObject));
                }
            }
        }

        CreateSkillLog();
    }

    private void JoshuaSkill()
    {
        print("joshuaSkill");
        if (skillData.targetIdList.Count > 0)
        {
            if (skillData.targetIdList.Contains(user.socketId))
            {
                user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(JOSHUA_BUFF_ID).InitializeBuff(user.gameObject));
            }

            //foreach (Player p in NetworkManager.instance.GetPlayerList())
            //{
            //    if (!skillData.targetIdList.Contains(p.socketId)) continue;

            //    //이펙트 재생시 여기서
            //}
        }


        CreateSkillLog();
    }

    private void RaiSkill()
    {
        print("raiSkill");

        if(skillData.targetId.Equals(user.socketId))
        {
            user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(RAI_BUFF_ID).InitializeBuff(user.gameObject));
            MissionPanel.Instance.Close();
        }
        //else if(playerList.TryGetValue(skillData.targetId,out Player p))
        //{
        //    //이펙트 재생시 여기에서
        //}
        CreateSkillLog();
    }

    private void CreateSkillLog()
    {
        Init();

        if (user.socketId.Equals(skillData.useSkillPlayerId))
        {
            //플레이어가 스킬 사용했을 시
            LogPanel.Instance.CreateLogText($"{user.socketName} -> {skillData.skillName} 사용");
        }
        else if (playerList.TryGetValue(skillData.useSkillPlayerId, out Player p))
        {
            //다른유저가 스킬 사용했을 시
            LogPanel.Instance.CreateLogText($"{p.socketName} -> {skillData.skillName} 사용");
        }
    }
        
}
