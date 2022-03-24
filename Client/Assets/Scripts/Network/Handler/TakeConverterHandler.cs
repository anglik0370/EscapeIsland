using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeConverterHandler : IMsgHandler<Converter>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);

        generic.SetTakeConverterAfterItem(int.Parse(payload));
    }
}
