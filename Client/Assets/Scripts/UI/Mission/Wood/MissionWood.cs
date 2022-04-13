using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionWood : MonoBehaviour, IMission
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

    public void Init()
    {
        branchList.ForEach(x => x.Init());
        slotList.ForEach(x => x.Init());
    }

    private void CuttingBranch(Vector2 beginDragPoint, Vector2 endDragPoint)
    {
        BranchMObj proximateMObj = null;
        Vector2 proximatePoint = Vector2.zero;

        Vector2 centerPoint = Vector2.Lerp(beginDragPoint, endDragPoint, 0.5f);

        for (int i = 0; i < branchList.Count; i++)
        {
            if (branchList[i].IsDropped) continue;

            Vector2 intersectionPoint = GetInterSection(branchList[i].BeginPoint, branchList[i].EndPoint, beginDragPoint, endDragPoint);

            if(CheckIntersectionInRange(intersectionPoint, branchList[i].BeginPoint, branchList[i].EndPoint))
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

    private bool CheckIntersectionInRange(Vector2 intersection, Vector2 beginPoint, Vector2 endPoint)
    {
        bool isHorizontal = (endPoint.x - beginPoint.x) > (endPoint.y - endPoint.y);

        if(isHorizontal)
        {
            return !(intersection.x < beginPoint.x || intersection.x > endPoint.x);
        }
        else
        {
            return !(intersection.y < beginPoint.y || intersection.y > endPoint.y);
        }
    }

    private Vector2 GetInterSection(Vector2 beginPoint, Vector2 endPoint, Vector3 beginDragPoint, Vector3 endDragPoint)
    {
        float m1 = (endPoint.y - beginPoint.y) / (endPoint.x - beginPoint.x);
        float m2 = (endDragPoint.y - beginDragPoint.y) / (endDragPoint.x - beginDragPoint.x);

        float x = (beginDragPoint.y - beginPoint.y + (m1 * beginPoint.x) - (m2 * beginDragPoint.x)) / (m1 - m2);
        float y = ((m1 * x) - (m1 * beginPoint.x)) + beginPoint.y;

        return new Vector2(x, y);
    }
}
