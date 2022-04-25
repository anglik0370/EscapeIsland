using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncObjHandler : IMsgHandler<SyncObjs>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);
        print(payload);
        SyncObjVO vo = JsonUtility.FromJson<SyncObjVO>(payload);
        generic.SetSyncObj(vo);
    }
}
