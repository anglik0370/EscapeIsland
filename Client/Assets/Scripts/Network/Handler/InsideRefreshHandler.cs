using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideRefreshHandler : IMsgHandler<InsideRefresh>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);

        UserListVO vo = JsonUtility.FromJson<UserListVO>(payload);

        generic.SetInside(vo.dataList);
    }
}
