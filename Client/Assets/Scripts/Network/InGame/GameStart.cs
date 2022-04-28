using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : ISetAble
{
    private List<UserVO> userDataList;

    private bool needStartGame = false;


    protected override void Start()
    {
        base.Start();
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

        Init();

        foreach (UserVO uv in userDataList)
        {
            if (uv.socketId == user.socketId)
            {
                user.transform.position = uv.position;
                user.isKidnapper = uv.isImposter;

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
        EventManager.OccurGameStart(user);
    }
}
