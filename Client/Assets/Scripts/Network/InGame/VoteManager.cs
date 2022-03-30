using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteManager : ISetAble
{
    public static VoteManager Instance { get; set; }

    private List<UserVO> userDataList;

    public VotePopup voteTab;

    private bool needVoteRefresh = false;
    private bool needTimerRefresh = false;
    private bool needTimeRefresh = false;
    private bool endVoteTime = false;
    private bool needVoteComplete = false;
    private bool needVoteDeadRefresh = false;

    public bool isVoteTime = false;
    private bool isTextChange = false;

    private TimeVO timeVO;
    private TimerVO timerVO;
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

    public static void SetVoteEnd()
    {
        lock (Instance.lockObj)
        {
            Instance.endVoteTime = true;
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

    public static void SetTimerData(TimerVO vo)
    {
        lock (Instance.lockObj)
        {
            Instance.needTimerRefresh = true;
            Instance.timerVO = vo;
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
    }

    void Update()
    {
        if (needVoteRefresh)
        {
            OnVoteTimeStart();
            needVoteRefresh = false;
        }

        if (needTimerRefresh)
        {
            TimerText();
            needTimerRefresh = false;
        }

        if (needTimeRefresh)
        {
            RefreshTime(timeVO.day, timeVO.isLightTime);
            needTimeRefresh = false;
        }
        if (endVoteTime)
        {
            EndVoteTime();
            endVoteTime = false;
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
        isTextChange = true;
        voteTab.ChangeMiddleText(msg);

        yield return new WaitForSeconds(1f);

        isTextChange = false;
    }

    public void TimerText()
    {
        if (isTextChange) return;

        string text = timerVO.curTime.ToString();

        if(timerVO.isInGameTimer)
        {
            TimeHandler.Instance.ChangeInGameTimeText(timerVO.curTime);
        }
        else
        {
            voteTab.ChangeMiddleText(text);
        }
    }

    public void OnVoteTimeStart()
    {
        Init();
        isVoteTime = true;

        EventManager.OccurStartMeet(meetingType);
        StartCoroutine(TextChange("��ǥ�ð� ����"));

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