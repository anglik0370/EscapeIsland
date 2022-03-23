using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour,ISetAble
{
    private Dictionary<int, Player> playerList;
    private List<UserVO> tempDataList;
    private Player user = null;


    private bool needStartGame = false;
    private bool once = false;

    private int socketId;

    private object lockObj = new object();

    void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            user = p;
        });
    }

    void Update()
    {
        if (needStartGame)
        {
            OnGameStart();

            needStartGame = false;
        }
    }

    public void SetGameStart(List<UserVO> list)
    {
        lock (lockObj)
        {
            tempDataList = list;
            needStartGame = true;
        }
    }

    public void OnGameStart()
    {
        PopupManager.instance.ClosePopup();

        playerList = NetworkManager.instance.GetPlayerDic();

        if (!once)
        {
            socketId = NetworkManager.instance.socketId;
        }

        foreach (UserVO uv in tempDataList)
        {
            if (uv.socketId == socketId)
            {
                user.transform.position = uv.position;
                user.isKidnapper = uv.isImposter;

                EventManager.OccurGameStart(user);
            }
            else
            {
                Player p = null;

                playerList.TryGetValue(uv.socketId, out p);

                if (p != null)
                {
                    p.transform.position = uv.position;
                    p.isKidnapper = uv.isImposter;
                }
            }
        }
    }
}
