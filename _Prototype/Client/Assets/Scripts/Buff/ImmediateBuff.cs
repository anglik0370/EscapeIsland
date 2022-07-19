using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmediateBuff : TimedBuff
{
    private readonly Player player;

    public ImmediateBuff(BuffSO buff, GameObject obj) : base(buff, obj)
    {
        player = obj.GetComponent<Player>();
    }

    public override void End()
    {
        player.SetImmediate(false);
    }

    protected override void ApplyEffect()
    {
        player.SetImmediate(true);
    }
}
