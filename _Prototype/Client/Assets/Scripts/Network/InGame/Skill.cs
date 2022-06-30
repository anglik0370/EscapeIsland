using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ISetAble
{
    public static Skill Instance { get; private set; }

    private bool isDissRapRefresh = false;
    private bool isRemoveAllDebuff = false;
    private bool isCherrySkill = false;
    private bool isJoshuaSkill = false;
    private bool isRaiSkill = false;
    private bool needSkillRefresh = false;

    private Team skillUseTeam = Team.NONE;
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
        if(isDissRapRefresh)
        {
            DissRap();
            isDissRapRefresh = false;
        }

        if(isRemoveAllDebuff)
        {
            RemoveAllDebuff();
            isRemoveAllDebuff = false;
        }

        if(isCherrySkill)
        {
            CherrySkill();
            isCherrySkill = false;
        }

        if(isJoshuaSkill)
        {
            JoshuaSkill();
            isJoshuaSkill = false;
        }

        if(isRaiSkill)
        {
            RaiSkill();
            isRaiSkill = false;
        }

        if(needSkillRefresh)
        {
            SkillRefresh();
            needSkillRefresh = false;
        }
    }

    public static void SetDissRap()
    {
        lock(Instance.lockObj)
        {
            Instance.isDissRapRefresh = true;
        }
    }

    public static void SetRemoveAllDebuff()
    {
        lock(Instance.lockObj)
        {
            Instance.isRemoveAllDebuff = true;
        }
    }

    public static void SetCherrySkill(Team team)
    {
        lock(Instance.lockObj)
        {
            Instance.isCherrySkill = true;
            Instance.skillUseTeam = team;
        }
    }

    public static void SetJoshuaSkill()
    {
        lock(Instance.lockObj)
        {
            Instance.isJoshuaSkill = true;
        }
    }

    public static void SetRaiSkill()
    {
        lock(Instance.lockObj)
        {
            Instance.isRaiSkill = true;
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
            UIManager.Instance.AlertText("디스랩 시전", AlertType.GameEvent);
        }
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
    }

    private void CherrySkill()
    {
        print("cherrySKill");

        if (skillData.targetIdList.Count <= 0) return;

        if(skillData.targetIdList.Contains(user.socketId))
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

            if (skillData.targetIdList.Count <= 1) return;
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

    private void JoshuaSkill()
    {
        print("joshuaSkill");
        if (skillData.targetIdList.Count <= 0) return;

        if (skillData.targetIdList.Contains(user.socketId))
        {
            user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(JOSHUA_BUFF_ID).InitializeBuff(user.gameObject));

            if (skillData.targetIdList.Count <= 1) return;
        }

        //foreach (Player p in NetworkManager.instance.GetPlayerList())
        //{
        //    if (!skillData.targetIdList.Contains(p.socketId)) continue;

        //    //이펙트 재생시 여기서
        //}
    }

    private void RaiSkill()
    {
        print("raiSkill");

        Init();

        if(skillData.targetId.Equals(user.socketId))
        {
            user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(RAI_BUFF_ID).InitializeBuff(user.gameObject));
            MissionPanel.Instance.Close();
        }
        //else if(playerList.TryGetValue(skillData.targetId,out Player p))
        //{
        //    //이펙트 재생시 여기에서
        //}
    }
        
}
