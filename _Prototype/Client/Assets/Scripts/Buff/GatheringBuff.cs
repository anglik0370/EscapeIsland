using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringBuff : TimedBuff
{
    private readonly Player player;

    public GatheringBuff(BuffSO buff, GameObject obj) : base(buff, obj)
    {
        player = obj.GetComponent<Player>();
    }

    public override void End()
    {
        player.SetEnemyGathering(false);
    }

    protected override void ApplyEffect()
    {
        player.SetEnemyGathering(true);
    }
}
