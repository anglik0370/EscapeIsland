using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour,ISetAble
{
    private Dictionary<int, Player> playerList;
    private List<UserVO> userDataList;
    private Player user = null;


    private bool needStartGame = false;

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
            userDataList = list;
            needStartGame = true;
        }
    }

    public void OnGameStart()
    {
        PopupManager.instance.ClosePopup();

        playerList = NetworkManager.instance.GetPlayerDic();

        foreach (UserVO uv in userDataList)
        {
            if (uv.socketId == user.socketId)
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