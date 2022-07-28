using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshUI : ISetAble
{
    public static RefreshUI Instance { get; private set; }

    [SerializeField]
    private OccupyUI occupyUI;

    private LobbyUIVO lobbyUIVO;

    private OccupyListVO occupyData;

    private bool needLobbyUIRefresh = false;
    private bool needOccupyUIRefresh = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(needLobbyUIRefresh)
        {
            LobbyUIRefresh();
            needLobbyUIRefresh = false;
        }

        if(needOccupyUIRefresh)
        {
            OccupyUIRefresh();
            needOccupyUIRefresh = false;
        }
    }

    public static void SetRefreshUI(LobbyUIVO vo)
    {
        lock(Instance.lockObj)
        {
            Instance.lobbyUIVO = vo;
            Instance.needLobbyUIRefresh = true;
        }
    }

    public static void SetOccupyRefresh(OccupyListVO data)
    {
        lock(Instance.lockObj)
        {
            Instance.occupyData = data;
            Instance.needOccupyUIRefresh = true;
        }
    }

    private void OccupyUIRefresh()
    {
        if(!occupyData.isOpen)
        {
            occupyUI.DisableUI();
            return;
        }

        foreach (OccupyVO ov in occupyData.areaDataList)
        {
            if (ov.area != user.Area) continue;

            if (occupyUI.IsOpen)
            {
                occupyUI.UpdateUI(ov.redGauge, ov.blueGauge);
            }
            else
            {
                occupyUI.SetUI(ov.redGauge, ov.blueGauge, ov.areaName);
                occupyUI.EnableUI();
            }
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
                    user.ChangeUI(uv,true);
                }
            }
            else
            {
                if(playerList.TryGetValue(uv.socketId, out Player p))
                {
                    if (!uv.curTeam.Equals(Team.NONE))
                    {
                        p.ChangeUI(uv, true);
                    }
                }
            }
        }
    }
}
