using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshUserHandler : MonoBehaviour, IMsgHandler
{
    private bool once = false;
    private RefreshUsers users;
    public void HandleMsg(string payload)
    {
        if (!once)
        {
            users = NetworkManager.instance.FindSetDataScript<RefreshUsers>();
            once = true;
        }

        UserListVO userList = JsonUtility.FromJson<UserListVO>(payload);
        users.SetUserRefreshData(userList.dataList);
    }
}
