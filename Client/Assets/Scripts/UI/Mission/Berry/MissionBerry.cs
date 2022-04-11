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
    private BerryMObj curBerryMObj;

    [SerializeField]
    private BerryGhostMObj berryGhost;

    private void Awake()
    {
        basketTrmList = basketPosParent.GetComponentsInChildren<Transform>().ToList();
        basketTrmList.RemoveAt(0);
    }

    public void SetCurBerryObj(BerryMObj berryMObj)
    {
        curBerryMObj = berryMObj;

        berryGhost.Init(curBerryMObj.ItemSprite, curBerryMObj.Rect);
    }

    public void MoveBerryGhost(Vector3 point)
    {
        berryGhost.Move(point);
    }

    public void EndDrag(bool isBasket)
    {
        if(isBasket)
        {
            curBerryMObj.MoveBasketPoint(basketTrmList[curBerryMObj.BerryId].position);
            dropCount++;

            if(dropCount == basketTrmList.Count)
            {
                //다 담긴 경우 이제 가져갈 수 있는 배리로 바꿔주면 된다
            }
        }

        berryGhost.Disable();
    }
}
