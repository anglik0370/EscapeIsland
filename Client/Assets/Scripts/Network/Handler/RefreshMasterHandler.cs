using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshMasterHandler : IMsgHandler<RefreshMasters>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);
        UserListVO userList = JsonUtility.FromJson<UserListVO>(payload);
        generic.SetMasterRefreshData(userList.dataList);
    }
}
