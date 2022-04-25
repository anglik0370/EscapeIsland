using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillHandler : IMsgHandler<Kill>
{
    public override void HandleMsg(string payload)
    {
        UserListVO userList = JsonUtility.FromJson<UserListVO>(payload);

        base.HandleMsg(payload);

        generic.SetDieData(userList.dataList);
    }
}
