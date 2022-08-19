using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ISetAble
{
    public static Skill Instance { get; private set; }

    private bool needSkillRefresh = false;
    private bool needFlyPaperRefresh = false;

    private SkillVO skillData;
    private FlyPaperVO flyPaperData;

    [SerializeField] private const int JOSHUA_BUFF_ID = 3;
    [SerializeField] private const int RAI_BUFF_ID = 4;
    [SerializeField] private const int KION_BUFF_ID = 12;

    [SerializeField] private const int CHERRY_ENEMY_TEAM_DEBUFF_ID = 111;
    [SerializeField] private const int CHERRY_ENEMY_TEAM_DEBUFF_ID2 = 112;
    [SerializeField] private const int CHERRY_SAME_TEAM_BUFF_ID = 113;


    [Header("기온")]
    private List<FlyPaper> flyPaperList = new List<FlyPaper>();

    [SerializeField] private FlyPaper flyPaperPrefab;

    private int lastFlyPaperId = 1;

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

        if(needFlyPaperRefresh)
        {
            FlyPaperRefresh();
            needFlyPaperRefresh = false;
        }
    }
    public static void SetSkill(SkillVO vo)
    {
        lock(Instance.lockObj)
        {
            Instance.skillData = vo;
            Instance.needSkillRefresh = true;
        }
    }

    public static void EnterFlyPaper(FlyPaperVO vo)
    {
        lock(Instance.lockObj)
        {
            Instance.flyPaperData = vo;
            Instance.needFlyPaperRefresh = true;
        }
    }

    private void FlyPaperRefresh()
    {
        FlyPaper flyPaper = flyPaperList.Find(x => x.id.Equals(flyPaperData.id) && x.userId.Equals(flyPaperData.userId));

        if(flyPaper != null)
        {
            flyPaper.Init();
        }

        if (flyPaperData.socketId.Equals(user.socketId))
        {
            user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(KION_BUFF_ID).InitializeBuff(user.gameObject));
        }
        //else if(playerList.TryGetValue(flyPaperData.socketId,out Player p))
        //{
        //    //이펙트 재생시 여기에서
        //}
    }

    private void SkillRefresh()
    {
        switch (skillData.skillType)
        {
            case CharacterType.Amber:
                break;
            case CharacterType.Cherry:
                CherrySkill();
                break;
            case CharacterType.IAN:
                RemoveAllDebuff();
                break;
            case CharacterType.Joshua:
                JoshuaSkill();
                break;
            case CharacterType.Rai:
                RaiSkill();
                break;
            case CharacterType.Randy:
                DissRap();
                break;
            case CharacterType.Sarsu:
                break;
            case CharacterType.Wonsong:
                break;
            case CharacterType.Ander:
                break;
            case CharacterType.Simon:
                SimonSkill();
                break;
            case CharacterType.Kion:
                KionSkill();
                break;
            default:
                break;
        }
    }

    private void SimonSkill()
    {
        CreateSkillLog(false);

        ItemSO item =  ItemManager.Instance.FindItemSO(skillData.targetId);
        StorageManager.Instance.RemoveItem(skillData.team, item);

        if(user.socketId.Equals(skillData.useSkillPlayerId) && !user.inventory.IsAllSlotFull)
        {
            user.inventory.AddItem(item);
        }
    }

    private void KionSkill()
    {
        CreateSkillLog(false);

        FlyPaper paper = null;

        if (flyPaperList.Count > 0)
        {
            paper = flyPaperList.Find(x => !x.gameObject.activeSelf);
        }

        if(paper == null)
        {
            paper = Instantiate(flyPaperPrefab, transform);
            flyPaperList.Add(paper);
        }

        paper.id = lastFlyPaperId++;
        paper.userId = skillData.useSkillPlayerId;
        paper.team = skillData.team;
        paper.transform.position = skillData.point;
        paper.SetEnable();
    }

    private void DissRap()
    {
        CreateSkillLog(false);

        if (!user.CurTeam.Equals(skillData.team))
        {
            MissionPanel.Instance.CloseGetMissionPanel();
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

        CreateSkillLog(false);

        if (user.CurTeam.Equals(skillData.team))
        {
            user.BuffHandler.RemoveAllDebuff();
        }

    }

    private void CherrySkill()
    {
        print("cherrySKill");

        CreateSkillLog(false);

        if (skillData.targetIdList.Count <= 0) return;

        if (skillData.targetIdList.Contains(user.socketId))
        {
            if (user.CurTeam.Equals(skillData.team))
            {
                user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(CHERRY_SAME_TEAM_BUFF_ID).InitializeBuff(user.gameObject));
            }
            //else
            //{
            //    user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(CHERRY_ENEMY_TEAM_DEBUFF_ID).InitializeBuff(user.gameObject));
            //    user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(CHERRY_ENEMY_TEAM_DEBUFF_ID2).InitializeBuff(user.gameObject));
            //}

            if (skillData.targetIdList.Count <= 1) return;
        }

        foreach (Player p in playerList.Values)
        {
            if (!skillData.targetIdList.Contains(p.socketId)) continue;

            if (p.CurTeam.Equals(skillData.team))
            {
                p.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(CHERRY_SAME_TEAM_BUFF_ID).InitializeBuff(p.gameObject));
            }
            //else
            //{
            //    p.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(CHERRY_ENEMY_TEAM_DEBUFF_ID).InitializeBuff(p.gameObject));
            //    p.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(CHERRY_ENEMY_TEAM_DEBUFF_ID2).InitializeBuff(p.gameObject));
            //}
        }

    }

    private void JoshuaSkill()
    {
        print("joshuaSkill");
        CreateSkillLog(false);

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


    }

    private void RaiSkill()
    {
        print("raiSkill");
        CreateSkillLog(true);

        if (skillData.targetId.Equals(user.socketId))
        {
            user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(RAI_BUFF_ID).InitializeBuff(user.gameObject));
            MissionPanel.Instance.Close();
        }
        //else if(playerList.TryGetValue(skillData.targetId,out Player p))
        //{
        //    //이펙트 재생시 여기에서
        //}
    }

    private void CreateSkillLog(bool single)
    {
        Init();

        if(single)
        {
            if (user.socketId.Equals(skillData.useSkillPlayerId))
            {
                //플레이어가 스킬 사용했을 시
                LogPanel.Instance.SingleSkillLog(user, GetTargetPlayer(), skillData.skillName);
            }
            else if (playerList.TryGetValue(skillData.useSkillPlayerId, out Player p))
            {
                //다른유저가 스킬 사용했을 시
                LogPanel.Instance.SingleSkillLog(p, GetTargetPlayer(), skillData.skillName);
            }

        }
        else
        {
            if (user.socketId.Equals(skillData.useSkillPlayerId))
            {
                //플레이어가 스킬 사용했을 시
                LogPanel.Instance.GlobalSkillLog(user,skillData.skillName);
            }
            else if (playerList.TryGetValue(skillData.useSkillPlayerId, out Player p))
            {
                //다른유저가 스킬 사용했을 시
                LogPanel.Instance.GlobalSkillLog(p, skillData.skillName);
            }
        }
       
    }
        
    private Player GetTargetPlayer()
    {
        if(user.socketId.Equals(skillData.targetId))
        {
            return user;
        }

        return playerList.TryGetValue(skillData.targetId, out Player p) ? p : null;
    }
}
