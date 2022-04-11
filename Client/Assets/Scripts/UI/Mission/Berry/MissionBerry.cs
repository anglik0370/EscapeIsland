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
    private Transform slotParent;

    [SerializeField]
    private List<Transform> basketTrmList;
    [SerializeField]
    private List<BerryMObj> berryList;
    [SerializeField]
    private List<MissionDropItemSlot> berrySlotList;

    [SerializeField]
    private BasketMObj basketMObj;
    [SerializeField]
    private int dropCount;

    [SerializeField]
    private BerryMObj curBerryMObj;

    [SerializeField]
    private BerryGhostMObj berryGhost;

    [Header("잭팟 확률")]
    [SerializeField]
    private float jackPotPercent = 50;

    private void Awake()
    {
        basketTrmList = basketPosParent.GetComponentsInChildren<Transform>().ToList();
        basketTrmList.RemoveAt(0);

        basketMObj = basketPosParent.GetComponent<BasketMObj>();
        berryList = berryParent.GetComponentsInChildren<BerryMObj>().ToList();

        berrySlotList = slotParent.GetComponentsInChildren<MissionDropItemSlot>().ToList();
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        berryGhost.Disable(false);
        berryList.ForEach(x => x.Init());

        berrySlotList.ForEach(x => x.Disable());

        basketMObj.Init();

        dropCount = 0;
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

                if(UtilClass.GetResult(jackPotPercent))
                {
                    berrySlotList.ForEach(x =>
                    {
                        x.Init();
                        x.SetRaycastTarget(true);
                    });
                }
                else
                {
                    berrySlotList[0].Init();
                    berrySlotList[0].SetRaycastTarget(true);
                }

                berryGhost.Disable(true);

                basketMObj.Disable();
                berryList.ForEach(x => x.Disable());

                return;
            }
        }

        berryGhost.Disable(false);
    }
}
