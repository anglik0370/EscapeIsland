using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshMasters : ISetAble
{
    private bool needMasterRefresh = false;
    private bool isGameStart = false;

    private List<UserVO> userDataList;

    [SerializeField]
    private ReadyBtn readyBtn;

    protected override void Start()
    {
        base.Start();

        EventManager.SubGameStart(p =>
        {
            isGameStart = true;
        });

        EventManager.SubExitRoom(() =>
        {
            isGameStart = false;
        });

        EventManager.SubBackToRoom(() =>
        {
            isGameStart = false;
        });
    }

    void Update()
    {
        if (needMasterRefresh)
        {
            RefreshMaster();

            needMasterRefresh = false;
        }
    }

    public void SetMasterRefreshData(List<UserVO> list)
    {
        lock (lockObj)
        {
            userDataList = list;
            needMasterRefresh = true;
        }
    }
    public void RefreshMaster()
    {
        Init();

        foreach (UserVO uv in userDataList)
        {
            if (uv.socketId == socketId)
            {
                user.master = uv.master;

                if(user.master)
                {
                    user.UI.SetNameTextColor(Color.black);
                    user.TeamUI.SetReadyImg(true, true);
                    user.UI.ClearStateText();

                }
                //user.isImposter = uv.isImposter;
            }
            else
            {
                Player p = null;

                playerList.TryGetValue(uv.socketId, out p);

                if (p != null)
                {
                    p.master = uv.master;

                    if(p.master)
                    {
                        p.UI.ClearStateText();
                        p.UI.SetNameTextColor(Color.black);
                        p.TeamUI.SetReadyImg(true, true);
                    }
                    //p.isImposter = uv.isImposter;
                }
            }
        }
    }
}
