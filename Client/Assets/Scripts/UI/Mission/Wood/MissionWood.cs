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
        dragScreen.SubOnEndDrag(CuttinBranch);
    }

    public void Init()
    {
        branchList.ForEach(x => x.Init());
        slotList.ForEach(x => x.Init());
    }

    private void CuttinBranch(Vector2 beginDragPoint, Vector2 endDragPoint)
    {
        BranchMObj proximateMObj = null;
        Vector2 proximatePoint = Vector2.zero;

        Vector2 centerPoint = Vector2.Lerp(beginDragPoint, endDragPoint, 0.5f);

        for (int i = 0; i < branchList.Count; i++)
        {
            Vector2 intersectionPoint = GetInterSection(branchList[i].BeginPoint, branchList[i].EndPoint, beginDragPoint, endDragPoint);

            if (CheckIntersectionInRange(intersectionPoint, branchList[i].BeginPoint, branchList[i].EndPoint, branchList[i].Type))
            {
                if (proximateMObj == null)
                {
                    proximateMObj = branchList[i];
                    proximatePoint = intersectionPoint;
                }
                else
                {
                    if(Vector2.Distance(proximatePoint, centerPoint) > Vector2.Distance(intersectionPoint, centerPoint))
                    {
                        proximateMObj = branchList[i];
                        proximatePoint = intersectionPoint;
                    }
                }
            }
        }

        if(proximateMObj != null)
        {
            proximateMObj.Drop(dropTrmList[proximateMObj.Id].anchoredPosition);
        }
    }

    private bool CheckIntersectionInRange(Vector2 intersection, Vector2 begin, Vector2 end, BranchMObjType type)
    {
        if(type == BranchMObjType.Horizontal)
        {
            if (intersection.x < begin.x || intersection.x > end.x) return false;
        }
        else
        {
            if (intersection.y < begin.y || intersection.y > end.y) return false;
        }

        return true;
    }

    private Vector2 GetInterSection(Vector2 pos1, Vector2 pos2, Vector3 pos3, Vector3 pos4)
    {
        float m1 = (pos2.y - pos1.y) / (pos2.x - pos1.x);
        float m2 = (pos4.y - pos3.y) / (pos4.x - pos3.x);

        float x = (pos3.y - pos1.y + (m1 * pos1.x) - (m2 * pos3.x)) / (m1 - m2);
        float y = ((m1 * x) - (m1 * pos1.x)) + pos1.y;

        return new Vector2(x, y);
    }
}
