using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : ISetAble
{
    private List<UserVO> userDataList;

    private bool needDieRefresh = false;

    void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            user = p;
        });
    }

    void Update()
    {
        if (needDieRefresh)
        {
            RefreshDie();
            needDieRefresh = false;
        }
    }

    public void SetDieData(List<UserVO> list)
    {
        lock (lockObj)
        {
            userDataList = list;
            needDieRefresh = true;
        }
    }

    public void RefreshDie()
    {
        Init();

        foreach (UserVO uv in userDataList)
        {
            if (uv.socketId == user.socketId)
            {
                if (uv.isDie)
                {
                    user.SetDead();
                }

            }
            else
            {
                Player p = null;

                playerList.TryGetValue(uv.socketId, out p);

                if (p != null)
                {
                    if (uv.isDie)
                    {
                        p.SetDead();
                    }

                    if (p.gameObject.activeSelf && uv.isDie && !user.isDie)
                    {
                        p.SetDisable();
                        p.SetDeadBody();
                    }
                }
            }

        }
        NetworkManager.instance.PlayerEnable();
    }
}
