using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedCoolBuff : TimedBuff
{
    private readonly Player player;

    public TimedCoolBuff(BuffSO buff, GameObject obj) : base(buff, obj)
    {
        player = obj.GetComponent<Player>();
    }

    public override void End()
    {
        CoolTimeBuffSO so = (CoolTimeBuffSO)Buff;

        player.SetMagnification(player.CoolTimeMagnification / (so.magnification * effectStacks));
        effectStacks = 0;
    }

    protected override void ApplyEffect()
    {
        CoolTimeBuffSO so = (CoolTimeBuffSO)Buff;

        player.SetMagnification(player.CoolTimeMagnification * so.magnification);
    }
}
