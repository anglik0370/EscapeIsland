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

    protected override void Awake()
    {
        base.Awake();

        Instance = this;
    }

    public Transform GetParent(bool isBlue)
    {
        return isBlue ? blueTeamParent : redTeamParent;
    }
}
