using System.Collections.Generic;
using UnityEngine;

public abstract class ISetAble : MonoBehaviour
{
    protected Dictionary<int, Player> playerList;
    protected Player user = null;

    protected int socketId;
    private bool once = false;

    protected object lockObj = new object();

    protected virtual void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            user = p;
        });
    }

    protected void Init()
    {
        playerList = NetworkManager.instance.GetPlayerDic();

        if (!once)
        {
            socketId = NetworkManager.instance.socketId;
        }
    }
}
