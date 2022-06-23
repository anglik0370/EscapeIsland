using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpeedBuff : TimedBuff
{
    private readonly Player player;

    public TimedSpeedBuff(BuffSO buff, GameObject obj) : base(buff,obj)
    {
        player = obj.GetComponent<Player>();
    }

    public override void End()
    {
        SpeedBuffSO speedBuffSO = (SpeedBuffSO)Buff;
        player.SetSpeed(player.Speed / (speedBuffSO.magnification * effectStacks));
        effectStacks = 0;
    }

    protected override void ApplyEffect()
    {
        SpeedBuffSO speedBuffSO = (SpeedBuffSO)Buff;
        player.SetSpeed(player.Speed * speedBuffSO.magnification);
    }
}
