using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteManager : ISetAble
{
    public static VoteManager Instance { get; set; }

    private List<UserVO> userDataList;

    public VotePopup voteTab;

    private bool needVoteRefresh = false;
    private bool needTimeRefresh = false;
    private bool needVoteComplete = false;
    private bool needVoteDeadRefresh = false;

    public bool isVoteTime = false;
    public bool isTextChange = false;

    private Timer timer;
    public float voteTime = 0f;
    private TimeVO timeVO;
    private VoteCompleteVO voteCompleteVO;

    private MeetingType meetingType = MeetingType.EMERGENCY;
    public MeetingType MeetingType => meetingType;
    private int tempId = -1;

    public static void SetVoteDead(int deadId)
    {
        lock (Instance.lockObj)
        {
            Instance.needVoteDeadRefresh = true;
            Instance.tempId = deadId;
        }
    }
    public static void SetVoteComplete(VoteCompleteVO vo)
    {
        lock (Instance.lockObj)
        {
            Instance.voteCompleteVO = vo;
            Instance.needVoteComplete = true;
        }
    }

    public static void SetVoteTime(List<UserVO> list, int type)
    {
        lock (Instance.lockObj)
        {
            Instance.userDataList = list;
            Instance.meetingType = (MeetingType)type;
            Instance.needVoteRefresh = true;
        }
    }

    public static void SetTimeRefresh(TimeVO vo)
    {
        lock (Instance.lockObj)
        {
            Instance.timeVO = vo;
            Instance.needTimeRefresh = true;
        }
    }

    void Awake()
    {
        Instance = this;
    }
    protected override void Start()
    {
        base.Start();
        EventManager.SubBackToRoom(() => voteTab.VoteUIDisable());

        StartCoroutine(CoroutineHandler.Frame(() => timer = NetworkManager.instance.FindSetDataScript<Timer>()));
    }

    void Update()
    {
        if (needVoteRefresh)
        {
            OnVoteTimeStart();
            needVoteRefresh = false;
        }

        if (needTimeRefresh)
        {
            RefreshTime(timeVO.day, timeVO.isLightTime);
            needTimeRefresh = false;
        }

        if (needVoteComplete)
        {
            VoteComplete();
            needVoteComplete = false;
        }

        if (needVoteDeadRefresh)
        {
            SetDeadRefresh();
            needVoteDeadRefresh = false;
        }
    }

    public void SetDeadRefresh()
    {
        Init();

        if (tempId == socketId)
        {
            user.SetDead();

        }
        else if (playerList.ContainsKey(tempId))
        {
            Player p = playerList[tempId];

            p.SetDead();

            if (p.gameObject.activeSelf && p.isDie && !user.isDie)
            {
                p.SetDisable();
            }
        }
        NetworkManager.instance.PlayerEnable();
    }
    public void VoteComplete()
    {
        VoteUI ui = voteTab.FindVoteUI(voteCompleteVO.voterId);
        ui.VoteComplete();

        if (voteCompleteVO.voterId == socketId)
        {
            voteTab.CompleteVote();
        }
    }

    public void RefreshTime(int day, bool isLightTime)
    {
        EndVoteTime();
        TimeHandler.Instance.TimeRefresh(day, isLightTime);
    }
    public void EndVoteTime()
    {
        isVoteTime = false;

        TimeHandler.Instance.InitKillCool();

        PopupManager.instance.ClosePopup();
        voteTab.VoteUIDisable();
    }

    IEnumerator TextChange(string msg)
    {
        voteTab.ChangeMiddleText(msg);
        isTextChange = true;
        yield return new WaitForSeconds(1.5f);

        isTextChange = false;
    }

    public void OnVoteTimeStart()
    {
        Init();
        isVoteTime = true;

        if(meetingType.Equals(MeetingType.EMERGENCY))
        {
            Timer.Instance.isEmergencyAble = false;
            Timer.Instance.InitEmergencyCoolTime();

        }

        EventManager.OccurStartMeet(meetingType);
        StartCoroutine(TextChange("투표시간 시작"));

        foreach (UserVO uv in userDataList)
        {
            if (uv.socketId == socketId)
            {
                user.transform.position = uv.position;
                voteTab.SetVoteUI(uv.socketId, uv.name, user.curSO.profileImg);
            }
            else
            {
                Player p = null;

                playerList.TryGetValue(uv.socketId, out p);

                if (p != null)
                {
                    p.transform.position = uv.position;
                    voteTab.SetVoteUI(uv.socketId, uv.name, p.curSO.profileImg);
                }
            }
        }
    }
}
