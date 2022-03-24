using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinKidnapperHandler : MonoBehaviour, IMsgHandler
{
    private Win win = null;
    private bool once = false;
    public void HandleMsg(string payload)
    {
        if (!once)
        {
            win = NetworkManager.instance.FindSetDataScript<Win>();
            once = true;
        }

        GameOverVO vo = JsonUtility.FromJson<GameOverVO>(payload);
        win.SetWinUserData(vo.dataList, vo.gameOverCase);
    }
}
