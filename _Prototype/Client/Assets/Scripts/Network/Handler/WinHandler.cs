using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinHandler : IMsgHandler<Win>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);
        GameOverVO vo = JsonUtility.FromJson<GameOverVO>(payload);
        generic.SetWinUserData(vo.dataList, vo.gameOverCase);
    }
}
