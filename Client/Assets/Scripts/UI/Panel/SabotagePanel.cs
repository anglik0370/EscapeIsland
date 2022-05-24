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
}
