using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshLobbyUI : ISetAble
{
    public static RefreshLobbyUI Instance { get; private set; }

    private LobbyUIVO lobbyUIVO;

    private bool needUIRefresh = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(needUIRefresh)
        {
            LobbyUIRefresh();
            needUIRefresh = false;
        }
    }

    public static void SetRefreshUI(LobbyUIVO vo)
    {
        lock(Instance.lockObj)
        {
            Instance.lobbyUIVO = vo;
            Instance.needUIRefresh = true;
        }
    }

    private void LobbyUIRefresh()
    {
        Init();
        //room user count
        UIManager.Instance.SetUserCountText(lobbyUIVO.roomVO.curUserNum, lobbyUIVO.roomVO.userNum);

        foreach (UserVO uv in lobbyUIVO.dataList)
        {
            bool isBlue = uv.curTeam.Equals(Team.BLUE);
            if (uv.socketId == socketId)
            {
                if(!uv.curTeam.Equals(Team.NONE) && user != null)
                {
                    user.UI.SetTeamImgColor(isBlue ? Color.blue : Color.red);
                    user.TeamUI.SetParent(TeamPanel.Instance.GetParent(isBlue));
                }
            }
            else
            {
                if(playerList.TryGetValue(uv.socketId, out Player p))
                {
                    if (!uv.curTeam.Equals(Team.NONE))
                    {
                        p.UI.SetTeamImgColor(isBlue ? Color.blue : Color.red);
                        p.TeamUI.SetParent(TeamPanel.Instance.GetParent(isBlue));
                    }
                }
            }
        }
    }
}
