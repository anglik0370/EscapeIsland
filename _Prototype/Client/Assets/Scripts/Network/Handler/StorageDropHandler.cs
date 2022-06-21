using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageDropHandler : IMsgHandler<ItemAndStorage>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);

        ItemStorageVO vo = JsonUtility.FromJson<ItemStorageVO>(payload);
        generic.SetItemStorage(vo);
    }
}
