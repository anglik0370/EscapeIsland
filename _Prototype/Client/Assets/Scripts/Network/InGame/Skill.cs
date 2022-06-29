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

    private Team skillUseTeam = Team.NONE;

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

    private void DissRap()
    {
        MissionPanel.Instance.CloseGetMissionPanel();
        UIManager.Instance.AlertText("µð½º·¦ ½ÃÀü", AlertType.GameEvent);
    }

    private void RemoveAllDebuff()
    {
        user.BuffHandler.RemoveAllDebuff();
    }

    private void CherrySkill()
    {
        print("cherrySKill");
        if(user.CurTeam.Equals(skillUseTeam))
        {
            user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(CHERRY_SAME_TEAM_BUFF_ID).InitializeBuff(user.gameObject));
        }
        else
        {
            user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(CHERRY_ENEMY_TEAM_DEBUFF_ID).InitializeBuff(user.gameObject));
            user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(CHERRY_ENEMY_TEAM_DEBUFF_ID2).InitializeBuff(user.gameObject));
        }
    }

    private void JoshuaSkill()
    {
        print("joshuaSkill");
        user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(JOSHUA_BUFF_ID).InitializeBuff(user.gameObject));
    }

    private void RaiSkill()
    {
        print("raiSkill");
        user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(RAI_BUFF_ID).InitializeBuff(user.gameObject));
        MissionPanel.Instance.Close();
    }
}
