using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCitizenHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        GameOverVO vo = JsonUtility.FromJson<GameOverVO>(payload);
        NetworkManager.SetWinUserData(vo.dataList, vo.gameOverCase);
    }
}
