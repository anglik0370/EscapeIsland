using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControl : TimedBuff
{
    private Player player;

    public CrowdControl(BuffSO buff, GameObject obj) : base(buff, obj)
    {
        player = obj.GetComponent<Player>();
    }

    public override void End()
    {
        player.canMove = true;
    }

    protected override void ApplyEffect()
    {
        player.canMove = false;
    }
}
