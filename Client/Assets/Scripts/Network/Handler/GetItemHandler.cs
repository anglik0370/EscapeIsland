using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemHandler : IMsgHandler<ItemAndStorage>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);
        int idx = int.Parse(payload);
        NetworkManager.instance.FindSetDataScript<ItemAndStorage>().SetItemDisable(idx);
    }
}
