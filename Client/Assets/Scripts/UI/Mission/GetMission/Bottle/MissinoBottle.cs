using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissinoBottle : MonoBehaviour, IGetMission
{
    private CanvasGroup cvs;
    public CanvasGroup Cvs => cvs;

    [SerializeField]
    private MissionType missionType;
    public MissionType MissionType => missionType;

    [SerializeField]
    private Transform slotParentTrm;
    private List<MissionDropItemSlot> slotList;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();

        slotList = slotParentTrm.GetComponentsInChildren<MissionDropItemSlot>().ToList();
    }

    public void Open()
    {
        
    }

    public void Close()
    {
        slotList.ForEach(x => x.Init());
        slotList.ForEach(x => x.SetRaycastTarget(true));
    }
}
