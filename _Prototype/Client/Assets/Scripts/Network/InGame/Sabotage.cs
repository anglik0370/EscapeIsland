using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Sabotage : ISetAble
{
    public static Sabotage Instance { get; private set; }

    private List<SabotageSO> sabotageSOList;

    [SerializeField]
    private GameObject trapPrefab;
    [SerializeField]
    private SabotageCallbackObj callbackObj;

    private SabotageVO sabotageData;

    private const int ANDER_ID = 8; //¿£´õ 

    private bool needSabotageRefresh = false;
    private bool needTrapRefresh = false;
    private bool needCantUseRefineryRefresh = false;
    private bool needExtinguishRefresh = false;
    private bool needArsonRefresh = false;

    private int lastTrapIdx = 1;

    private List<Trap> trapList = new List<Trap>();
    private List<LabDoor> doorList = new List<LabDoor>();

    private CantUseRefineryVO refineryData;
    private TrapVO trapData;
    private ArsonVO arsonData;

    [SerializeField]
    private Transform doorParent;

    public bool CanEmergency => !ArsonManager.Instance.isArson && ConvertPanel.Instance.EndSabotage();


    private void Awake()
    {
        Instance = this;

        PoolManager.CreatePool<Trap>(trapPrefab, transform, 10);

        EventManager.SubGameOver(goc => InitTrap());
        EventManager.SubExitRoom(() => InitTrap());

        sabotageSOList = Resources.LoadAll<SabotageSO>("SabotageSO/").ToList();
    }

    protected override void Start()
    {
        base.Start();

        //for (int i = 0; i < doorParent.childCount; i++)
        //{
        //    doorList.Add(doorParent.GetChild(i).GetComponentInChildren<LabDoor>());
        //}

        Instantiate(callbackObj, transform);
    }

    private void Update()
    {
        if (needSabotageRefresh)
        {
            StartSabotage();
            needSabotageRefresh = false;
        }

        if (needTrapRefresh)
        {
            EnterTrap();
            needTrapRefresh = false;
        }

        if (needCantUseRefineryRefresh)
        {
            SetRefinery();
            needCantUseRefineryRefresh = false;
        }

        if (needExtinguishRefresh)
        {
            SetExtinguish();
            needExtinguishRefresh = false;
        }

        if(needArsonRefresh)
        {
            SetArson();
            needArsonRefresh = false;
        }
    }

    public static void SetSabotageData(SabotageVO vo)
    {
        lock (Instance.lockObj)
        {
            Instance.sabotageData = vo;
            Instance.needSabotageRefresh = true;
        }
    }

    public static void SetTrapData(TrapVO vo)
    {
        lock (Instance.lockObj)
        {
            Instance.trapData = vo;
            Instance.needTrapRefresh = true;
        }
    }

    public static void SetRefineryData(CantUseRefineryVO vo)
    {
        lock (Instance.lockObj)
        {
            Instance.refineryData = vo;
            Instance.needCantUseRefineryRefresh = true;
        }
    }

    public static void SetExtinguishData()
    {
        lock (Instance.lockObj)
        {
            Instance.needExtinguishRefresh = true;
        }
    }

    public static void SetArsonData(ArsonVO vo)
    {
        lock(Instance.lockObj)
        {
            Instance.arsonData = vo;
            Instance.needArsonRefresh = true;
        }
    }

    public void SetExtinguish()
    {
        ArsonManager.Instance.SlotActive(false);
    }

    private void SetArson()
    {
        StorageManager.Instance.RemoveItem(arsonData.team, ItemManager.Instance.FindItemSO(arsonData.itemSOId));
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
        ArsonManager.Instance.isBlue = sabotageData.team.Equals(Team.BLUE);

        SabotageSO so = GetSabotageSO(sabotageData.sabotageName);
        so.callback?.Invoke();

        UIManager.Instance.AlertText(sabotageData.sabotageName, AlertType.Warning);
    }

    public void SpawnTrap()
    {
        Trap trap = PoolManager.GetItem<Trap>();

        if (!trapList.Contains(trap))
            trapList.Add(trap);

        foreach (UserVO uv in sabotageData.userDataList)
        {
            if (uv.socketId == sabotageData.starterId)
            {
                trap.transform.position = uv.position;
                trap.id = lastTrapIdx++;
                trap.team = sabotageData.team;
                break;
            }
        }

        trap.SetEnable();
    }

    public void EnterTrap()
    {
        if(user.socketId.Equals(trapData.socketId))
        {
            user.BuffHandler.AddBuff(BuffManager.Instance.GetBuffSO(ANDER_ID).InitializeBuff(user.gameObject));
        }

        Trap trap = FindTrap(trapData.trapId);

        if (trap != null)
        {
            trap.EnterTrap(trapData.socketId);
        }
    }

    public void InitTrap()
    {
        lastTrapIdx = 1;
        trapList.ForEach(trap => trap.Init());
    }

    public Trap FindTrap(int trapIdx)
    {
        return trapList.Find(x => x.id == trapIdx);
    }

    public List<Trap> GetTrapList()
    {
        return trapList;
    }

    public void CloseDoor()
    {
        for (int i = 0; i < doorList.Count; i++)
        {
            doorList[i].CloseDoor();
        }
    }

    public SabotageSO GetSabotageSO(string sabotageName)
    {
        return sabotageSOList.Find(x => x.sabotageName.Equals(sabotageName));
    }
}
