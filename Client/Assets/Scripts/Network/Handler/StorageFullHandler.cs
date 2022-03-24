using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageFullHandler : IMsgHandler<ItemAndStorage>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);
        generic.SetStorageFullData(payload);
    }
}
