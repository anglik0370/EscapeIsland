using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SabotagePanel : Panel
{
    public static SabotagePanel Instance { get; private set; }

    private SabotageButton sabotageBtnPrefab;
    private List<SabotageSO> sabotageSOList;
    private List<SabotageButton> sabotageList = new List<SabotageButton>();
    public List<SabotageButton> SabotageList => sabotageList;

    private const float OTHER_SABOTAGE_USE_COOLTIME = 20f;
    private const float EMERGENCY_USE_COOLTIME = 30f;

    private string[] sharedSabotagesName;

    protected override void Awake()
    {
        Instance = this;

        sabotageBtnPrefab = Resources.Load<SabotageButton>("UI/SabotageBtn");

        sabotageSOList = Resources.LoadAll<SabotageSO>("SabotageSO/").ToList();

        for (int i = 0; i < sabotageSOList.Count; i++)
        {
            SabotageButton btn = Instantiate(sabotageBtnPrefab, transform);
            btn.Init(sabotageSOList[i]);
            sabotageList.Add(btn);
        }

        sharedSabotagesName = new string[2]
        {
            "방화","필터 고장"
        };

        base.Awake();
    }

    protected override void Start()
    {
        EventManager.SubGameStart(p =>
        {
            if (p.isKidnapper)
            {
                Open(true);
                GameStart();
            }
            else
            {
                Close(false);
            }
        });

        EventManager.SubExitRoom(() => Close(false));
        EventManager.SubBackToRoom(() => Close(false));
        EventManager.SubGameOver(goc => Close(false));
        EventManager.SubStartMeet(mt =>
        {
            StartVoteTime();
        });
    }

    public void GameStart()
    {
        for (int i = 0; i < sabotageList.Count; i++)
        {
            sabotageList[i].StartTimer();
        }
    }

    public SabotageButton FindSabotageButton(string sabotageName)
    {
        return sabotageList.Find(x => x.SabotageSO.sabotageName == sabotageName);
    }

    public void UseSabotage(SabotageButton useSabotage)
    {
        for (int i = 0; i < sabotageList.Count; i++)
        {
            if (sabotageList[i].Equals(useSabotage) || sabotageList[i].CurCoolTime >= OTHER_SABOTAGE_USE_COOLTIME) continue;

            sabotageList[i].StartSabotageCoolTime(OTHER_SABOTAGE_USE_COOLTIME);
        }
    }

    public void StartVoteTime()
    {
        for (int i = 0; i < sabotageList.Count; i++)
        {
            if (sabotageList[i].CurCoolTime >= EMERGENCY_USE_COOLTIME) continue;

            sabotageList[i].StartSabotageCoolTime(EMERGENCY_USE_COOLTIME);
        }
    }

    public bool SharedSabotage(string sabotageName)
    {
        for (int i = 0; i < sharedSabotagesName.Length; i++)
        {
            if(sharedSabotagesName[i].Equals(sabotageName))
            {
                return true;
            }
        }
        return false;
    }
}
