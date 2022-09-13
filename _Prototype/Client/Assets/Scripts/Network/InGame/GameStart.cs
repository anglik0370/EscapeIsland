using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : ISetAble
{
    public static GameStart Instance { get; private set; }

    private List<UserVO> userDataList;
    private UserVO userVO;

    private bool needStartGame = false;
    private bool needReadyRefresh = false;

    private Coroutine co;

    private void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (needStartGame)
        {
            OnGameStart();

            needStartGame = false;
        }

        if(needReadyRefresh)
        {
            ReadyRefresh();
            needReadyRefresh = false;
        }
    }

    public static void SetGameStart(List<UserVO> list)
    {
        lock (Instance.lockObj)
        {
            Instance.userDataList = list;
            Instance.needStartGame = true;
        }
    }

    public static void SetGameReady(UserVO vo)
    {
        lock(Instance.lockObj)
        {
            Instance.userVO = vo;
            Instance.needReadyRefresh = true;
        }
    }

    private void ReadyRefresh()
    {
        Init();

        if(userVO.socketId == user.socketId)
        {
            user.isReady = userVO.ready;
            user.UI.SetNameTextColor(userVO.ready ? Color.black : Color.gray);
            user.TeamUI.SetReadyImg(userVO.ready, false);

            user.SetReadyText(user.isReady);
            return;
        }

        playerList.TryGetValue(userVO.socketId, out Player p);

        if(p != null)
        {
            p.isReady = userVO.ready;
            p.TeamUI.SetReadyImg(userVO.ready, false);
            p.UI.SetNameTextColor(userVO.ready ? Color.black : Color.gray);

            p.SetReadyText(p.isReady);
        }
    }

    public void OnGameStart()
    {
        PopupManager.instance.ClosePopup();

        Init();

        foreach (UserVO uv in userDataList)
        {
            if (uv.socketId == user.socketId)
            {
                if (co != null)
                {
                    StopCoroutine(co);
                }

                co = StartCoroutine(CoroutineHandler.EnableDampingEndFrame(GameManager.Instance.CmVCam));

                user.transform.position = uv.position;

                //if (uv.curTeam != Team.NONE)
                //    user.UI.SetNameTextColor(uv.curTeam.Equals(Team.BLUE) ? Color.blue : Color.red);
            }
            else
            {
                Player p = null;

                playerList.TryGetValue(uv.socketId, out p);

                if (p != null)
                {
                    p.SetPosition(uv.position);

                    //if(uv.curTeam != Team.NONE)
                    //    p.UI.SetNameTextColor(uv.curTeam.Equals(Team.BLUE) ? Color.blue : Color.red);
                }
            }
        }

        EventManager.OccurGameInit();
        EventManager.OccurGameStart(user);
    }
}
