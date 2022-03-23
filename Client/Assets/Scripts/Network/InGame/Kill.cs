using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : MonoBehaviour,ISetAble
{
    private Dictionary<int, Player> playerList;
    private List<UserVO> userDataList;

    private Player user = null;

    private bool needDieRefresh = false;
    private bool once = false;

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
        playerList = NetworkManager.instance.GetPlayerDic();

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
