using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartHandler : IMsgHandler<GameStart>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);
        UserListVO vo = JsonUtility.FromJson<UserListVO>(payload);
        generic.SetGameStart(vo.dataList);
    }
}
