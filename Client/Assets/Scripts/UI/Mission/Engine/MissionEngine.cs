using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MissionEngine : MonoBehaviour, IMission
{
    private CanvasGroup cvs;
    public CanvasGroup Cvs => cvs;

    private DragScreen dragScreen;

    [SerializeField]
    private Transform wireParentTrm;
    private List<WireMObj> wireList;

    [SerializeField]
    private Transform wireOrderImgParentTrm;
    private List<Image> wireOrderImgList;

    [SerializeField]
    private CanvasGroup cvsEngineSlot;
    [SerializeField]
    private MissionDropItemSlot slot;

    [SerializeField]
    private MissionType missionType;
    public MissionType MissionType => missionType;

    [Header("전선을 잘 끊고 있는지")]
    private int curOrder;
    public int CurOder => curOrder;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();

        dragScreen = GetComponentInChildren<DragScreen>();

        wireList = wireParentTrm.GetComponentsInChildren<WireMObj>().ToList();
        wireOrderImgList = wireOrderImgParentTrm.GetComponentsInChildren<Image>().ToList();
    }

    private void Start()
    {
        //dragScreen.SubOnEndDrag(CutWire);
    }

    public void Init()
    {
        curOrder = 1;

        UtilClass.SetCanvasGroup(cvsEngineSlot);
        slot.Init();
        slot.SetRaycastTarget(true);

        wireList.ForEach(x => x.Init());

        List<int> orderList = new List<int>(4) { 1, 2, 3, 4 };

        for (int i = 0; i < 4; i++)
        {
            int idx = UnityEngine.Random.Range(0, orderList.Count);

            wireList[i].SetCuttingOrder(orderList[idx]);
            wireOrderImgList[orderList[idx] - 1].sprite = wireList[i].Sprite;

            orderList.RemoveAt(idx);
        }
    }

    public void AddOder()
    {
        curOrder++;
    }

    private void CutWire(Vector2 beginDragPoint, Vector2 endDragPoint)
    {
        WireMObj proximateMObj = null;
        Vector2 proximatePoint = Vector2.zero;

        Vector2 centerPoint = Vector2.Lerp(beginDragPoint, endDragPoint, 0.5f);

        for (int i = 0; i < wireList.Count; i++)
        {
            if (wireList[i].IsCut) continue;

            Vector2 intersectionPoint = UtilClass.GetInterSection(wireList[i].BeginPoint, wireList[i].EndPoint, beginDragPoint, endDragPoint);

            if (UtilClass.CheckIntersectionInRange(intersectionPoint, wireList[i].BeginPoint, wireList[i].EndPoint))
            {
                if (proximateMObj == null)
                {
                    proximateMObj = wireList[i];
                    proximatePoint = intersectionPoint;
                }
                else
                {
                    if (Vector2.Distance(proximatePoint, centerPoint) > Vector2.Distance(intersectionPoint, centerPoint))
                    {
                        proximateMObj = wireList[i];
                        proximatePoint = intersectionPoint;
                    }
                }
            }
        }

        if (proximateMObj == null)
        {
            return;
        }

        if(proximateMObj.CuttingOrder == curOrder)
        {
            proximateMObj.Cut();
            curOrder++;

            if(curOrder > 4)
            {
                UtilClass.SetCanvasGroup(cvsEngineSlot, 1, true, true, false);
            }
        }
    }
}
