using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamPanel : Panel
{
    public static TeamPanel Instance { get; private set; }

    private Player user;

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

        EventManager.SubEnterRoom(p =>
        {
            user = p;
        });

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

        SoundManager.Instance.PlayBtnSfx();
        base.Open(isTweenSkip);
    }

    public override void Close(bool isTweenSkip = false)
    {
        base.Close(isTweenSkip);
        SoundManager.Instance.PlayBtnSfx();
    }

    private void SendChangeTeam(bool isBlueTeam)
    {
        if (user.isReady)
        {
            UIManager.Instance.AlertText("준비 중엔 바꿀 수 없습니다.", AlertType.Warning);
            return;
        }

        team = isBlueTeam ? Team.BLUE : Team.RED;

        if (team.Equals(NetworkManager.instance.User.CurTeam)) return;

        TeamVO vo = new TeamVO(team);
        DataVO dataVO = new DataVO("CHANGE_TEAM", JsonUtility.ToJson(vo));
        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }
}
