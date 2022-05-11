using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Sabotage : ISetAble
{
    public static Sabotage Instance { get; private set; }

    [SerializeField]
    private GameObject trapPrefab;

    private SabotageVO sabotageData;

    private bool needSabotageRefresh = false;
    private bool needTrapRefresh = false;
    private bool needCantUseRefineryRefresh = false;
    private bool needExtinguishRefresh = false;

    private int trapId = -1;
    private int lastTrapIdx = 1;

    private List<Trap> trapList = new List<Trap>();
    private List<LabDoor> doorList = new List<LabDoor>();

    private CantUseRefineryVO refineryData;
    private ObjVO extinguishData;

    [SerializeField]
    private Transform doorParent;


    private void Awake()
    {
        Instance = this;

        PoolManager.CreatePool<Trap>(trapPrefab, transform, 10);

        EventManager.SubGameOver(goc => lastTrapIdx = 1);
        EventManager.SubExitRoom(() => lastTrapIdx = 1);
    }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (needSabotageRefresh)
        {
            StartSabotage();
            needSabotageRefresh = false;
        }

        if(needTrapRefresh)
        {
            EnterTrap();
            needTrapRefresh = false;
        }

        if(needCantUseRefineryRefresh)
        {
            SetRefinery();
            needCantUseRefineryRefresh = false;
        }

        if(needExtinguishRefresh)
        {
            SetExtinguish();
            needExtinguishRefresh = false;
        }
    }

    public static void SetSabotageData(SabotageVO vo)
    {
        lock(Instance.lockObj)
        {
            Instance.sabotageData = vo;
            Instance.needSabotageRefresh = true;
        }
    }
    
    public static void SetTrapData(int trapId)
    {
        lock(Instance.lockObj)
        {
            Instance.trapId = trapId;
            Instance.needTrapRefresh = true;
        }
    }

    public static void SetRefineryData(CantUseRefineryVO vo)
    {
        lock(Instance.lockObj)
        {
            Instance.refineryData = vo;
            Instance.needCantUseRefineryRefresh = true;
        }
    }

    public static void SetExtinguishData(ObjVO vo)
    {
        lock(Instance.lockObj)
        {
            Instance.extinguishData = vo;
            Instance.needExtinguishRefresh = true;
        }
    }

    public void SetExtinguish()
    {
        ArsonSlot slot = ArsonManager.Instance.GetArsonSlot(extinguishData.objId);

        //¿œ¥‹ ≤Ù±‚∏∏
        slot.gameObject.SetActive(false);
    }

    public void SetRefinery()
    {
        ItemConverter converter = ConverterManager.Instance.GetRefinery(refineryData.refineryId);

        converter.isEmpty[refineryData.slotIdx] = false;
        converter.CanUse = converter.CanUseConverter();

        ConvertPanel.Instance.UpdateUIs();
    }

    public void StartSabotage()
    {
        SabotageButton curSabotage = SabotagePanel.Instance.FindSabotageButton(sabotageData.sabotageName);

        if((sabotageData.isShareCoolTime && user.isKidnapper) || user.socketId == sabotageData.starterId)
        {
            curSabotage.StartSabotage(sabotageData.isShareCoolTime ? curSabotage.SabotageSO.shareCoolTime : curSabotage.SabotageSO.coolTime);
        }

        curSabotage.SabotageSO.callback?.Invoke();
        print("Start SAbotage");
    }

    public void SpawnTrap()
    {
        Trap trap = PoolManager.GetItem<Trap>();

        if(!trapList.Contains(trap))
            trapList.Add(trap);

        foreach (UserVO uv in sabotageData.userDataList)
        {
            if(uv.socketId == sabotageData.starterId)
            {
                trap.transform.position = uv.position;
                trap.id = lastTrapIdx++;
                break;
            }
        }
    }

    public void EnterTrap()
    {
        Trap trap = FindTrap(trapId);

        if(trap != null)
        {
            trap.EnterTrap();
        }
    }

    public Trap FindTrap(int trapIdx)
    {
        return trapList.Find(x => x.id == trapIdx);
    }

    public void CloseDoor()
    {
        for (int i = 0; i < doorList.Count; i++)
        {
            doorList[i].CloseDoor();
        }
    }
}
