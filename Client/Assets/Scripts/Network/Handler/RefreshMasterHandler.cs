using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshMasterHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        UserListVO userList = JsonUtility.FromJson<UserListVO>(payload);
        NetworkManager.SetMasterRefreshData(userList.dataList);
    }
}
