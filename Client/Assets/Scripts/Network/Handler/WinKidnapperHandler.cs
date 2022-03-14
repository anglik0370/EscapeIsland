using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinKidnapperHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        GameOverVO vo = JsonUtility.FromJson<GameOverVO>(payload);
        NetworkManager.SetWinUserData(vo.dataList, vo.gameOverCase);
    }
}
