using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartHandler : MonoBehaviour, IMsgHandler
{
    private GameStart gs = null;
    private bool once = false;

    public void HandleMsg(string payload)
    {
        UserListVO vo = JsonUtility.FromJson<UserListVO>(payload);

        if(!once)
        {
            gs = NetworkManager.instance.FindSetDataScript<GameStart>();
            once = true;
        }

        gs.SetGameStart(vo.dataList);
    }
}
