using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissinoBottle : MonoBehaviour, IMission
{
    private CanvasGroup cvs;
    public CanvasGroup Cvs => cvs;

    [SerializeField]
    private MissionType missionType;
    public MissionType MissionType => missionType;

    [SerializeField]
    private Transform slotParentTrm;
    private List<MissionDropItemSlot> slotLost;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();

        slotLost = slotParentTrm.GetComponentsInChildren<MissionDropItemSlot>().ToList();
    }

    public void Init()
    {
        slotLost.ForEach(x => x.Init());
    }
}
