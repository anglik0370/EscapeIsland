using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshMasterHandler : MonoBehaviour, IMsgHandler
{
    private RefreshMasters refreshMaster = null;
    private bool once = false;
    public void HandleMsg(string payload)
    {
        if(!once)
        {
            refreshMaster = NetworkManager.instance.FindSetDataScript<RefreshMasters>();
            once = true;
        }
        UserListVO userList = JsonUtility.FromJson<UserListVO>(payload);
        refreshMaster.SetMasterRefreshData(userList.dataList);
    }
}
