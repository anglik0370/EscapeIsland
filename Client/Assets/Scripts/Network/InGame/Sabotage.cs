using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sabotage : ISetAble
{
    public static Sabotage Instance { get; private set; }

    [SerializeField]
    private GameObject trapPrefab;

    private Dictionary<string, Action> sabotageDic = new Dictionary<string, Action>();

    private SabotageVO sabotageData;

    private bool needSabotageRefresh = false;
    private bool needTrapRefresh = false;

    private int trapId = -1;

    private List<Trap> trapList = new List<Trap>();
    private int lastTrapIdx = 1;

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

        List<SabotageButton> sabotageList = SabotagePanel.Instance.SabotageList;
        for (int i = 0; i < sabotageList.Count; i++)
        {
            int idx = i;
            sabotageDic.Add(sabotageList[idx].SabotageSO.sabotageName, () =>
            {
                Invoke(sabotageList[idx].SabotageSO.callbackName,0f);
            });
        }
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

    public void StartSabotage()
    {
        SabotageButton curSabotage = SabotagePanel.Instance.FindSabotageButton(sabotageData.sabotageName);

        if((sabotageData.isShareCoolTime && user.isKidnapper) || user.isKidnapper)
        {
            curSabotage.StartSabotage();
        }

        sabotageDic[curSabotage.SabotageSO.sabotageName]?.Invoke();
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
}
