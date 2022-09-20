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
    [SerializeField] private const int IAN_BUFF_ID = 6;
    [SerializeField] private const int KION_SPEED_DEBUFF_ID = 12;
    [SerializeField] private const int KION_COOLTIME_DEBUFF_ID = 13;

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
        if (needSkillRefresh)
        {
            SkillRefresh();
            needSkillRefresh = false;
        }

        if (needFlyPaperRefresh)
        {
            FlyPaperRefresh();
            needFlyPaperRefresh = false;
        }
    }
    public static void SetSkill(SkillVO vo)
    {
        lock (Instance.lockObj)
        {
            Instance.skillData = vo;
            Instance.needSkillRefresh = true;
        }
    }

    public static void EnterFlyPaper(FlyPaperVO vo)
    {
        lock (Instance.lockObj)
        {
            Instance.flyPaperData = vo;
            Instance.needFlyPaperRefresh = true;
        }
    }

    private void FlyPaperRefresh()
    {
        FlyPaper flyPaper = flyPaperList.Find(x => x.id.Equals(flyPaperData.id) && x.userId.Equals(flyPaperData.userId));
        BuffSO coolDebuffSO = BuffManager.Instance.GetBuffSO(KION_COOLTIME_DEBUFF_ID);

        if (flyPaper != null)
        {
            flyPaper.Init();
        }

        if (flyPaperData.socketId.Equals(user.socketId))
        {
            user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(KION_SPEED_DEBUFF_ID).InitializeBuff(user.gameObject));
            user.BuffHandler.AddBuff(coolDebuffSO.InitializeBuff(user.gameObject));
            user.UI.SetState("미션 쿨타임 증가", UtilClass.GetStateColor(coolDebuffSO.isBuffed));
        }
        else if (playerList.TryGetValue(flyPaperData.socketId, out Player p))
        {
            p.UI.SetState("미션 쿨타임 증가", UtilClass.GetStateColor(coolDebuffSO.isBuffed));
            //이펙트 재생시 여기에서
        }
    }

    private void SkillRefresh()
    {
        switch (skillData.skillType)
        {
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
            case CharacterType.Simon:
                SimonSkill();
                break;
            case CharacterType.Leon:
                LeonSkill();
                break;
            case CharacterType.Kion:
                KionSkill();
                break;
            case CharacterType.Amber:
                AmberSkill();
                break;
            default:
                CreateSkillLog(false);
                break;
        }
    }

    private void AmberSkill()
    {
        CreateSkillLog(false);

        ConvertPanel.Instance.ConvertLimit(skillData.team);
    }

    private void LeonSkill()
    {
        CreateSkillLog(true);

        ItemSO item = ItemManager.Instance.FindItemSO(skillData.itemId);

        if (item != null)
        {
            if (user.socketId.Equals(skillData.useSkillPlayerId) && !user.inventory.IsAllSlotFull)
            {
                user.inventory.AddItem(item);
            }
            else if (user.socketId.Equals(skillData.targetId))
            {
                user.UI.SetState("재료 빼앗김", UtilClass.GetStateColor(false));
                user.inventory.RemoveItem(item);
            }
        }
    }

    

    private void SimonSkill()
    {
        CreateSkillLog(false);

        ItemSO item = ItemManager.Instance.FindItemSO(skillData.targetId);
        ItemAmount curAmount = StorageManager.Instance.FindItemAmount(false, skillData.team, item);

        if (curAmount.amount > 0)
        {
            StorageManager.Instance.RemoveItem(skillData.team, item);

            if (user.socketId.Equals(skillData.useSkillPlayerId) && !user.inventory.IsAllSlotFull)
            {
                user.inventory.AddItem(item);
            }
        }
        else
        {
            Debug.LogError("아이템이 없음");
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

        if (paper == null)
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
        CreateSkillLog(false);

        List<Trap> trapList = Sabotage.Instance.GetTrapList();
        List<Trap> idList = new List<Trap>();

        foreach (Trap trap in trapList)
        {
            if (trap.enterPlayerId != -1 && trap.gameObject.activeSelf)
            {
                idList.Add(trap);
            }
        }

        Trap t = null;

        if (user.CurTeam.Equals(skillData.team))
        {
            t = idList.Find(x => x.enterPlayerId.Equals(user.socketId));

            if (t != null && user.CurTeam.Equals(skillData.team))
            {
                t.Init();
            }

            if (user.BuffHandler.IsDebuffed())
            {
                user.UI.SetState("디버프 해제", UtilClass.GetStateColor(true));
                user.BuffHandler.RemoveAllDebuff();
            }
            else
            {
                user.UI.SetState("빨라짐", UtilClass.GetStateColor(true));
                user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(IAN_BUFF_ID).InitializeBuff(user.gameObject));
            }
        }

        foreach (Player p in NetworkManager.instance.GetPlayerList())
        {
            if (p.CurTeam.Equals(skillData.team))
            {
                t = idList.Find(x => x.enterPlayerId.Equals(p.socketId));

                if (t != null)
                {
                    t.Init();
                }

                if (p.BuffHandler.IsDebuffed())
                {
                    p.UI.SetState("디버프 해제", UtilClass.GetStateColor(true));
                    p.BuffHandler.RemoveAllDebuff();
                }
                else
                {
                    p.UI.SetState("빨라짐", UtilClass.GetStateColor(true));
                    p.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(IAN_BUFF_ID).InitializeBuff(p.gameObject));
                }
            }
        }
    }

    private void CherrySkill()
    {
        print("cherrySKill");

        CreateSkillLog(false);

        if (skillData.targetIdList.Count <= 0) return;

        BuffSO buffSO = BuffManager.Instance.GetBuffSO(CHERRY_SAME_TEAM_BUFF_ID);

        if (skillData.targetIdList.Contains(user.socketId))
        {
            if (user.CurTeam.Equals(skillData.team))
            {
                user.UI.SetState("미션 쿨타임 감소", UtilClass.GetStateColor(buffSO.isBuffed));
                user.BuffHandler.AddBuff(buffSO.InitializeBuff(user.gameObject));
            }

            if (skillData.targetIdList.Count <= 1) return;
        }

        foreach (Player p in playerList.Values)
        {
            if (!skillData.targetIdList.Contains(p.socketId)) continue;

            if (p.CurTeam.Equals(skillData.team))
            {
                p.UI.SetState("미션 쿨타임 감소", UtilClass.GetStateColor(buffSO.isBuffed));
                p.BuffHandler.AddBuff(buffSO.InitializeBuff(p.gameObject));
            }
        }
    }

    private void JoshuaSkill()
    {
        print("joshuaSkill");
        CreateSkillLog(false);

        if(skillData.useSkillPlayerId.Equals(user.socketId))
        {
            for (int i = 0; i < skillData.itemIdList.Count; i++)
            {
                if (user.inventory.IsAllSlotFull) break;

                ItemSO so = ItemManager.Instance.FindItemSO(skillData.itemIdList[i]);

                if(so != null)
                {
                    user.inventory.AddItem(so);
                }
            }
        }

        if (skillData.targetIdList.Count > 0)
        {
            BuffSO buff = BuffManager.Instance.GetBuffSO(JOSHUA_BUFF_ID);
            if (skillData.targetIdList.Contains(user.socketId))
            {
                int idx = skillData.targetIdList.FindIndex(x => x.Equals(user.socketId));
                ItemSO so = ItemManager.Instance.FindItemSO(skillData.itemIdList[idx]);
                
                if(so != null)
                {
                    user.inventory.RemoveItem(so);
                }

                user.UI.SetState("이동 불가", UtilClass.GetStateColor(buff.isBuffed));
                user.BuffHandler.AddBuff(buff.InitializeBuff(user.gameObject));
            }

            foreach (Player p in NetworkManager.instance.GetPlayerList())
            {
                if (!skillData.targetIdList.Contains(p.socketId)) continue;

                //이펙트 재생시 여기서
                p.UI.SetState("이동 불가", UtilClass.GetStateColor(buff.isBuffed));
            }
        }
    }

    private void RaiSkill()
    {
        print("raiSkill");
        CreateSkillLog(true);

        BuffSO buff = BuffManager.Instance.GetBuffSO(RAI_BUFF_ID);

        if (skillData.targetId.Equals(user.socketId))
        {
            user.UI.SetState("기절", UtilClass.GetStateColor(buff.isBuffed));
            user.BuffHandler.AddBuff(buff.InitializeBuff(user.gameObject));
            MissionPanel.Instance.Close();
        }
        else if (playerList.TryGetValue(skillData.targetId, out Player p))
        {
            p.UI.SetState("기절", UtilClass.GetStateColor(buff.isBuffed));
            //이펙트 재생시 여기에서
        }
    }

    private void CreateSkillLog(bool single)
    {
        Init();

        if (single)
        {
            print("싱글 로그");

            if (user.socketId.Equals(skillData.useSkillPlayerId))
            {
                //플레이어가 스킬 사용했을 시
                LogPanel.Instance.SingleSkillLog(user, GetTargetPlayer(), skillData.skillName);
                SoundManager.Instance.PlayCharacterSound(user.curSO.skill.skillSFX, user);
            }
            else if (playerList.TryGetValue(skillData.useSkillPlayerId, out Player p))
            {
                //다른유저가 스킬 사용했을 시
                LogPanel.Instance.SingleSkillLog(p, GetTargetPlayer(), skillData.skillName);
                SoundManager.Instance.PlayCharacterSound(p.curSO.skill.skillSFX, p);
            }

        }
        else
        {
            print("글로벌 로그");

            if (user.socketId.Equals(skillData.useSkillPlayerId))
            {
                //플레이어가 스킬 사용했을 시
                LogPanel.Instance.GlobalSkillLog(user, skillData.skillName);
                SoundManager.Instance.PlayCharacterSound(user.curSO.skill.skillSFX, user);
            }
            else if (playerList.TryGetValue(skillData.useSkillPlayerId, out Player p))
            {
                //다른유저가 스킬 사용했을 시
                LogPanel.Instance.GlobalSkillLog(p, skillData.skillName);
                SoundManager.Instance.PlayCharacterSound(p.curSO.skill.skillSFX, p);
            }
        }

    }

    private Player GetTargetPlayer()
    {
        if (user.socketId.Equals(skillData.targetId))
        {
            return user;
        }

        return playerList.TryGetValue(skillData.targetId, out Player p) ? p : null;
    }
}
