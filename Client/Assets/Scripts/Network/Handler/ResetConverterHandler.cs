using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetConverterHandler : IMsgHandler<Converter>
{
    public override void HandleMsg(string payload)
    {
        base.HandleMsg(payload);
        generic.SetResetConverter(int.Parse(payload));
    }
}
