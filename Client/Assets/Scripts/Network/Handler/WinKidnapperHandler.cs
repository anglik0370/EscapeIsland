using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinKidnapperHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        UserListVO vo = JsonUtility.FromJson<UserListVO>(payload);
        NetworkManager.SetWinUserData(vo.dataList, true);
    }
}
