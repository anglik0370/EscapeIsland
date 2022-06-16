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
        //room user count
        UIManager.Instance.SetUserCountText(lobbyUIVO.roomVO.curUserNum, lobbyUIVO.roomVO.userNum);


    }
}
