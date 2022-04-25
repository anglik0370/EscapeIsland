using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionOre : MonoBehaviour, IMission
{
    private CanvasGroup cvs;
    public CanvasGroup Cvs => cvs;

    private MissionPanel missionPanel;

    [SerializeField]
    private MissionType missionType;
    public MissionType MissionType => missionType;

    [SerializeField]
    private Transform oreParentTrm;
    [SerializeField]
    private List<OreAreaMObj> oreMObjList;

    [SerializeField]
    private Transform slotParentTrm;
    [SerializeField]
    private List<MissionDropItemSlot> slotList;

    [SerializeField]
    private OreAreaMObj curOreArea;
    public OreAreaMObj CurOreArea => curOreArea;

    [SerializeField]
    private int getItemCnt;

    [SerializeField]
    private float spawnPercentage = 60f;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();

        missionPanel = GetComponentInParent<MissionPanel>();

        oreMObjList = oreParentTrm.GetComponentsInChildren<OreAreaMObj>().ToList();
        slotList = slotParentTrm.GetComponentsInChildren<MissionDropItemSlot>().ToList();
    }

    public void Init()
    {
        getItemCnt = 0;

        curOreArea = null;

        oreMObjList.ForEach(x => x.Init());
        slotList.ForEach(x => x.Disable());
        slotList.ForEach(x => x.SetRaycastTarget(false));

        InitSlot();
    }

    private void InitSlot()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            if (UtilClass.GetResult(spawnPercentage))
            {
                slotList[i].Init();
                slotList[i].SetRaycastTarget(true);
            }
        }
    }

    public void OnGetItem()
    {
        getItemCnt++;

        if (getItemCnt == 3)
        {
            missionPanel.Close(true);

            return;
        }
    }

    public void SetCurOreArea(OreAreaMObj oreAreaMObj)
    {
        curOreArea = oreAreaMObj;
    }
}
