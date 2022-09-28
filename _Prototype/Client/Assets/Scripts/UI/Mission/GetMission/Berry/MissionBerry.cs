using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionBerry : MonoBehaviour, IGetMission
{
    private CanvasGroup cvs;
    public CanvasGroup Cvs => cvs;

    [Header("������Ʈ ������ �� ���� �θ� Transform")]
    [SerializeField]
    private Transform basketPosParent;
    [SerializeField]
    private Transform berryParent;
    [SerializeField]
    private Transform slotParent;

    private List<Transform> basketTrmList;
    private List<BerryMObj> berryList;
    private List<MissionDropItemSlot> berrySlotList;

    [Header("�ٱ��� ����")]
    [SerializeField]
    private BasketMObj basketMObj;
    [SerializeField]
    private int dropCount;

    [Header("���� �̵� ����")]
    [SerializeField]
    private BerryMObj curBerryMObj;
    [SerializeField]
    private BerryGhostMObj berryGhost;

    [Header("�̼� Ÿ��")]
    [SerializeField]
    private MissionType missionType;
    public MissionType MissionType => missionType;

    [Header("���� Ȯ��")]
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

    public void Open()
    {
        
    }

    public void Close()
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
                //�� ��� ��� ���� ������ �� �ִ� �踮�� �ٲ��ָ� �ȴ�

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
