using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldRayCaster : GraphicRaycaster
{
    [SerializeField]
    private int sortOrder = 0;

    public override int sortOrderPriority => sortOrder;
}
