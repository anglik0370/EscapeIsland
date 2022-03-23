using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillHandler : MonoBehaviour, IMsgHandler
{
    private bool once = false;
    private Kill kill = null;
    public void HandleMsg(string payload)
    {
        UserListVO userList = JsonUtility.FromJson<UserListVO>(payload);

        if (!once)
        {
            kill = NetworkManager.instance.FindSetDataScript<Kill>();
            once = true;
        }

        kill.SetDieData(userList.dataList);
    }
}
