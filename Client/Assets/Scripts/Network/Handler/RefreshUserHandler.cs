using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshUserHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        UserListVO userList = JsonUtility.FromJson<UserListVO>(payload);
        
        NetworkManager.SetUserRefreshData(userList.dataList);
    }
}
