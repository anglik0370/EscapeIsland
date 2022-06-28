using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControl : TimedBuff
{
    private readonly Player player;

    public CrowdControl(BuffSO buff, GameObject obj) : base(buff, obj)
    {
        player = obj.GetComponent<Player>();
    }

    public override void End()
    {
        CrowdControlSO so = (CrowdControlSO)Buff;
        player.canMove = true;

        if (!so.isRestrict) return;

        if (player.IsRestrict)
        {
            if (duration > 0f)
            {
                player.StartOffRestrict(duration);
                return;
            }

            player.SetRestrict(false);
        }
    }


    protected override void ApplyEffect()
    {
        CrowdControlSO so = (CrowdControlSO)Buff;

        player.canMove = false;

        if (so.isRestrict)
        {
            player.SetRestrict(true);
        }
    }
}
