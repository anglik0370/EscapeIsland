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


    [Header("���")]
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
            user.UI.SetState("�̼� ��Ÿ�� ����", UtilClass.GetStateColor(coolDebuffSO.isBuffed));
        }
        else if (playerList.TryGetValue(flyPaperData.socketId, out Player p))
        {
            p.UI.SetState("�̼� ��Ÿ�� ����", UtilClass.GetStateColor(coolDebuffSO.isBuffed));
            //����Ʈ ����� ���⿡��
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
                user.UI.SetState("��� ���ѱ�", UtilClass.GetStateColor(false));
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
            Debug.LogError("�������� ����");
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
                user.UI.SetState("����� ����", UtilClass.GetStateColor(true));
                user.BuffHandler.RemoveAllDebuff();
            }
            else
            {
                user.UI.SetState("������", UtilClass.GetStateColor(true));
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
                    p.UI.SetState("����� ����", UtilClass.GetStateColor(true));
                    p.BuffHandler.RemoveAllDebuff();
                }
                else
                {
                    p.UI.SetState("������", UtilClass.GetStateColor(true));
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
                user.UI.SetState("�̼� ��Ÿ�� ����", UtilClass.GetStateColor(buffSO.isBuffed));
                user.BuffHandler.AddBuff(buffSO.InitializeBuff(user.gameObject));
            }

            if (skillData.targetIdList.Count <= 1) return;
        }

        foreach (Player p in playerList.Values)
        {
            if (!skillData.targetIdList.Contains(p.socketId)) continue;

            if (p.CurTeam.Equals(skillData.team))
            {
                p.UI.SetState("�̼� ��Ÿ�� ����", UtilClass.GetStateColor(buffSO.isBuffed));
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

                user.UI.SetState("�̵� �Ұ�", UtilClass.GetStateColor(buff.isBuffed));
                user.BuffHandler.AddBuff(buff.InitializeBuff(user.gameObject));
            }

            foreach (Player p in NetworkManager.instance.GetPlayerList())
            {
                if (!skillData.targetIdList.Contains(p.socketId)) continue;

                //����Ʈ ����� ���⼭
                p.UI.SetState("�̵� �Ұ�", UtilClass.GetStateColor(buff.isBuffed));
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
            user.UI.SetState("����", UtilClass.GetStateColor(buff.isBuffed));
            user.BuffHandler.AddBuff(buff.InitializeBuff(user.gameObject));
            MissionPanel.Instance.Close();
        }
        else if (playerList.TryGetValue(skillData.targetId, out Player p))
        {
            p.UI.SetState("����", UtilClass.GetStateColor(buff.isBuffed));
            //����Ʈ ����� ���⿡��
        }
    }

    private void CreateSkillLog(bool single)
    {
        Init();

        if (single)
        {
            print("�̱� �α�");

            if (user.socketId.Equals(skillData.useSkillPlayerId))
            {
                //�÷��̾ ��ų ������� ��
                LogPanel.Instance.SingleSkillLog(user, GetTargetPlayer(), skillData.skillName);
                SoundManager.Instance.PlayCharacterSound(user.curSO.skill.skillSFX, user);
            }
            else if (playerList.TryGetValue(skillData.useSkillPlayerId, out Player p))
            {
                //�ٸ������� ��ų ������� ��
                LogPanel.Instance.SingleSkillLog(p, GetTargetPlayer(), skillData.skillName);
                SoundManager.Instance.PlayCharacterSound(p.curSO.skill.skillSFX, p);
            }

        }
        else
        {
            print("�۷ι� �α�");

            if (user.socketId.Equals(skillData.useSkillPlayerId))
            {
                //�÷��̾ ��ų ������� ��
                LogPanel.Instance.GlobalSkillLog(user, skillData.skillName);
                SoundManager.Instance.PlayCharacterSound(user.curSO.skill.skillSFX, user);
            }
            else if (playerList.TryGetValue(skillData.useSkillPlayerId, out Player p))
            {
                //�ٸ������� ��ų ������� ��
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
