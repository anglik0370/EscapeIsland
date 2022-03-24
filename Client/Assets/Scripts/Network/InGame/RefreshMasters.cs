using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshMasters : ISetAble
{
    private bool needMasterRefresh = false;

    private List<UserVO> userDataList;

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
                //user.isImposter = uv.isImposter;
            }
            else
            {
                Player p = null;

                playerList.TryGetValue(uv.socketId, out p);

                if (p != null)
                {
                    p.master = uv.master;
                    //p.isImposter = uv.isImposter;
                }
            }
        }
    }
}
