using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamPanel : Panel
{
    public static TeamPanel Instance { get; private set; }

    [SerializeField]
    private Transform redTeamParent;
    [SerializeField]
    private Transform blueTeamParent;

    private bool isGameStart = false;

    protected override void Awake()
    {
        base.Awake();

        Instance = this;
    }

    protected override void Start()
    {
        base.Start();

        EventManager.SubGameStart(p =>
        {
            isGameStart = true;
        });

        EventManager.SubGameOver(goc =>
        {
            isGameStart = false;
        });
    }

    public Transform GetParent(bool isBlue)
    {
        return isBlue ? blueTeamParent : redTeamParent;
    }

    public override void Open(bool isTweenSkip = false)
    {
        if (isGameStart) return;

        base.Open(isTweenSkip);
    }
}
