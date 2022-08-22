using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/AreaRestrictionSkillSO", fileName = "New AreaRestrictionSkillSO")]
public class AreaRestrictionSkillSO : SkillSO
{
    public List<Collider2D> colliderList;

    public bool isInShip;

    public override void UpdateTimer()
    {
        if (isInShip) return;

        base.UpdateTimer();
    }
}
