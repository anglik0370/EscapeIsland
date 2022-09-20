using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AltarVO : VO
{
    public int id;
    public int altarBuffId;

    public AltarVO(int id, int altarBuffId)
    {
        this.id = id;
        this.altarBuffId = altarBuffId;
    }
}
