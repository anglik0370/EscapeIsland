using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ISetAble
{
    public static Skill Instance { get; private set; }

    private bool isDissRapRefresh = false;
    private bool isRemoveAllDebuff = false;
    private bool isCherrySkill = false;

    private Team skillUseTeam = Team.NONE;

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
        if(user.CurTeam.Equals(skillUseTeam))
        {

        }
        else
        {

        }
    }
}
