using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : ISetAble
{
    private bool needWinRefresh = false;

    private List<UserVO> gameOverUserList;
    private GameOverCase gameOverCase = GameOverCase.BlueWin;

    private Coroutine co;

    void Update()
    {
        if (needWinRefresh)
        {
            SetWinTeam();
            needWinRefresh = false;
        }
    }

    public void SetWinUserData(List<UserVO> list, GameOverCase gameOverCase)
    {
        lock (lockObj)
        {
            needWinRefresh = true;
            gameOverUserList = list;
            this.gameOverCase = gameOverCase;
        }
    }
    public void SetWinTeam()
    {
        //이긴 팀에 따라 해줘야 할 일 해주기
        Init();
        user.canMove = false;
        VoteManager.Instance.EndVoteTime();
        bool isBlueWin = gameOverCase == GameOverCase.BlueWin;

        foreach (UserVO uv in gameOverUserList)
        {
            bool isWinTeam = (isBlueWin && uv.curTeam.Equals(Team.BLUE)) || (!isBlueWin && uv.curTeam.Equals(Team.RED));
            if (uv.socketId == socketId)
            {
                if(isWinTeam)
                    GameOverPanel.Instance.MakeWinImg(user);

                if (co != null)
                {
                    StopCoroutine(co);
                }

                co = StartCoroutine(CoroutineHandler.EnableDampingEndFrame(GameManager.Instance.CmVCam));
                user.transform.position = uv.position;
            }
            else
            {
                Player p = null;

                playerList.TryGetValue(uv.socketId, out p);

                if (p != null)
                {
                    if (isWinTeam)
                        GameOverPanel.Instance.MakeWinImg(p);
                    p.SetPosition(uv.position);
                }
            }
        }

        EventManager.OccurGameOver(gameOverCase);

        
    }
}
