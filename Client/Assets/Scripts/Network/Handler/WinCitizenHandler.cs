using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCitizenHandler : IMsgHandler<Win>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);
        GameOverVO vo = JsonUtility.FromJson<GameOverVO>(payload);
        generic.SetWinUserData(vo.dataList, vo.gameOverCase);
    }
}
