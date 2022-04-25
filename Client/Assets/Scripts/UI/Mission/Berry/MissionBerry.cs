using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionBerry : MonoBehaviour, IMission
{
    private CanvasGroup cvs;
    public CanvasGroup Cvs => cvs;

    [Header("컴포넌트 가져올 때 쓰는 부모 Transform")]
    [SerializeField]
    private Transform basketPosParent;
    [SerializeField]
    private Transform berryParent;
    [SerializeField]
    private Transform slotParent;

    private List<Transform> basketTrmList;
    private List<BerryMObj> berryList;
    private List<MissionDropItemSlot> berrySlotList;

    [Header("바구니 관련")]
    [SerializeField]
    private BasketMObj basketMObj;
    [SerializeField]
    private int dropCount;

    [Header("베리 이동 관련")]
    [SerializeField]
    private BerryMObj curBerryMObj;
    [SerializeField]
    private BerryGhostMObj berryGhost;

    [Header("미션 타입")]
    [SerializeField]
    private MissionType missionType;
    public MissionType MissionType => missionType;

    [Header("잭팟 확률")]
    [SerializeField]
    private float jackPotPercent = 50;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();

        basketTrmList = basketPosParent.GetComponentsInChildren<Transform>().ToList();
        basketTrmList.RemoveAt(0);

        basketMObj = basketPosParent.GetComponent<BasketMObj>();
        berryList = berryParent.GetComponentsInChildren<BerryMObj>().ToList();

        berrySlotList = slotParent.GetComponentsInChildren<MissionDropItemSlot>().ToList();
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
