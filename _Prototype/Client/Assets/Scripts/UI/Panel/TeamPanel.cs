using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamPanel : Panel
{
    public static TeamPanel Instance { get; private set; }

    [SerializeField]
    private Transform redTeamParent;
    [SerializeField]
    private Transform blueTeamParent;

    [SerializeField]
    private Button redTeamBtn;
    [SerializeField]
    private Button blueTeamBtn;

    private Team team = Team.NONE;

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
        EventManager.SubExitRoom(() => isGameStart = false);

        redTeamBtn.onClick.AddListener(() =>
        {
            SendChangeTeam(false);
        });
        blueTeamBtn.onClick.AddListener(() =>
        {
            SendChangeTeam(true);
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


    private void SendChangeTeam(bool isBlueTeam)
    {
        team = isBlueTeam ? Team.BLUE : Team.RED;

        if (team.Equals(NetworkManager.instance.User.CurTeam)) return;

        TeamVO vo = new TeamVO(team);
        DataVO dataVO = new DataVO("CHANGE_TEAM", JsonUtility.ToJson(vo));
        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }
}
