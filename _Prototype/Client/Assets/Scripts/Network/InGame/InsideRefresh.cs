using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideRefresh : ISetAble
{
    private bool needInsideRefresh = false;

    private List<UserVO> userDataList = null;

    public void SetInside(List<UserVO> list)
    {
        lock(lockObj)
        {
            userDataList = list;
            needInsideRefresh = true;
        }
    }

    void Update()
    {
        if(needInsideRefresh)
        {
            RefreshInside();
            needInsideRefresh = false;
        }
    }

    public void RefreshInside()
    {
        Init();

        foreach (UserVO vo in userDataList)
        {
            if (vo.socketId != socketId)
            {
                Player p = null;

                if (playerList.TryGetValue(vo.socketId, out p))
                {
                    p.Area = vo.area;
                }
            }
        }
    }
}
