using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshUserHandler : IMsgHandler<RefreshUsers>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);

        UserListVO userList = JsonUtility.FromJson<UserListVO>(payload);
        generic.SetUserRefreshData(userList.dataList);
    }
}
