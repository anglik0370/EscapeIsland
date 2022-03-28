using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : ISetAble
{
    private List<UserVO> userDataList;

    private bool needDieRefresh = false;

    protected override void Start()
    {
        base.Start();
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

    public void KillPlayer(Player targetPlayer)
    {
        int targetSocketId = 0;

        foreach (int socketId in playerList.Keys)
        {
            if (playerList[socketId] == targetPlayer)
            {
                targetSocketId = socketId;
                break;
            }
        }

        SendManager.Instance.SendKill(targetSocketId);
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
