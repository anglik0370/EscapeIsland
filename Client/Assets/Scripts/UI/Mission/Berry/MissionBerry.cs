using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionBerry : MonoBehaviour
{
    [SerializeField]
    private Transform basketPosParent;
    [SerializeField]
    private Transform berryParent;

    [SerializeField]
    private List<Transform> basketTrmList;
    [SerializeField]
    private int dropCount;

    [SerializeField]
    private BerryMObj curMoveingObj;

    private void Awake()
    {
        basketTrmList = basketPosParent.GetComponentsInChildren<Transform>().ToList();
    }

    public void SetCurMovingObj(BerryMObj berryMObj)
    {
        curMoveingObj = berryMObj;
    }

    public void MoveToBasketTrm()
    {
        if (curMoveingObj == null) return;

        curMoveingObj.transform.position = basketTrmList[dropCount].position;
        dropCount++;
    }
}
