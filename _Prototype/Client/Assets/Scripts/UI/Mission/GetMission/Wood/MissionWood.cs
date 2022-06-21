using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionWood : MonoBehaviour, IGetMission
{
    private DragScreen dragScreen;
    private CanvasGroup cvs;

    public CanvasGroup Cvs => cvs;

    [SerializeField]
    private Transform branchParent;
    [SerializeField]
    private List<BranchMObj> branchList;
    [SerializeField]
    private List<MissionDropItemSlot> slotList;

    [SerializeField]
    private Transform dropTrmParent;
    [SerializeField]
    private List<RectTransform> dropTrmList;

    private bool isOpen = false;
    public bool IsOpen => isOpen;

    [SerializeField]
    private MissionType missionType;
    public MissionType MissionType => missionType;

    private void Awake()
    {
        dragScreen = GetComponentInChildren<DragScreen>();
        cvs = GetComponent<CanvasGroup>();

        branchList = branchParent.GetComponentsInChildren<BranchMObj>().ToList();
        slotList = branchParent.GetComponentsInChildren<MissionDropItemSlot>().ToList();

        dropTrmList = dropTrmParent.GetComponentsInChildren<RectTransform>().ToList();
        dropTrmList.RemoveAt(0);
    }

    private void Start()
    {
        dragScreen.SubOnEndDrag(CuttingBranch);
    }

    public void Open()
    {
        isOpen = true;
        branchList.ForEach(x => x.Init());
        slotList.ForEach(x => x.Init());
    }

    public void Close()
    {
        branchList.ForEach(x => x.Init());
        slotList.ForEach(x => x.Init());
        isOpen = false;
    }

    private void CuttingBranch(Vector2 beginDragPoint, Vector2 endDragPoint)
    {
        BranchMObj proximateMObj = null;
        Vector2 proximatePoint = Vector2.zero;

        Vector2 centerPoint = Vector2.Lerp(beginDragPoint, endDragPoint, 0.5f);

        for (int i = 0; i < branchList.Count; i++)
        {
            if (branchList[i].IsDropped) continue;

            Vector2 intersectionPoint = UtilClass.GetInterSection(branchList[i].BeginPoint, branchList[i].EndPoint, beginDragPoint, endDragPoint);

            if(UtilClass.CheckIntersectionInRange(intersectionPoint, branchList[i].BeginPoint, branchList[i].EndPoint))
            {
                if (proximateMObj == null)
                {
                    proximateMObj = branchList[i];
                    proximatePoint = intersectionPoint;
                }
                else
                {
                    if (Vector2.Distance(proximatePoint, centerPoint) > Vector2.Distance(intersectionPoint, centerPoint))
                    {
                        proximateMObj = branchList[i];
                        proximatePoint = intersectionPoint;
                    }
                }
            }
        }

        if(proximateMObj == null)
        {
            return;
        }

        proximateMObj.Drop(dropTrmList[proximateMObj.Id].anchoredPosition);
    }
}
