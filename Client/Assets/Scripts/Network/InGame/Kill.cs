using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : ISetAble
{
    private KillVO data;

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

    public void SetDieData(KillVO data)
    {
        lock (lockObj)
        {
            this.data = data;
            needDieRefresh = true;
        }
    }

    public void KillPlayer(Player targetPlayer)
    {
        Init();

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

        Player p = null;

        if(playerList.TryGetValue(data.killerId, out p))
        {
            if(p != null)
            {
                p.SetAttack();
            }
        }

        if(playerList.TryGetValue(data.targetSocketId, out p))
        {
            if (p != null)
            {
                p.SetDead();

                if (p.gameObject.activeSelf && p.isDie && !user.isDie)
                {
                    p.SetDisable();
                    DeadBodyManager.Instance.MakeDeadbody(p.GetTrm().position);
                }
            }
        }
        else if(user.socketId == data.targetSocketId)
        {
            user.SetDead();
        }

        NetworkManager.instance.PlayerEnable();
    }
}
