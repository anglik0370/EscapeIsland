using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteManager : ISetAble
{
    public static VoteManager Instance { get; set; }

    private List<UserVO> userDataList;
    private Queue<ChatVO> chatQueue = new Queue<ChatVO>();

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
    private VoteCompleteVO voteCompleteVO;

    private MeetingType meetingType = MeetingType.EMERGENCY;
    private int curTime = -1;
    private int tempId = -1;

    public static void SetVoteDead(int deadId)
    {
        lock (Instance.lockObj)
        {
            Instance.needVoteDeadRefresh = true;
            Instance.tempId = deadId;
        }
    }

    public static void ReceiveChat(ChatVO vo)
    {
        lock (Instance.lockObj)
        {
            Instance.chatQueue.Enqueue(vo);
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

    public static void SetTimerData(int curTime)
    {
        lock (Instance.lockObj)
        {
            Instance.needTimerRefresh = true;
            Instance.curTime = curTime;
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
    void Start()
    {
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

        while (chatQueue.Count > 0)
        {
            ChatVO vo = chatQueue.Dequeue();
            print("ChatHandler");

            Player p = null;

            if (playerList.TryGetValue(vo.socketId, out p))
            {
                if ((!p.isDie && !user.isDie) || user.isDie)
                {
                    voteTab.CreateChat(false, p.socketName, vo.msg, p.curSO.profileImg);
                }
            }
            else
            {
                if (user.socketId == vo.socketId)
                {
                    voteTab.CreateChat(true, user.socketName, vo.msg, user.curSO.profileImg);
                }
            }
        }
    }

    public void SetDeadRefresh()
    {
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
        voteTab.ChangeMiddleText(curTime.ToString());
    }

    public void OnVoteTimeStart()
    {
        isVoteTime = true;

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
