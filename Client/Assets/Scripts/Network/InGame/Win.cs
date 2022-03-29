using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : ISetAble
{
    private bool needWinRefresh = false;

    private List<UserVO> gameOverUserList;
    private GameOverCase gameOverCase = GameOverCase.CollectAllItem;

    void Update()
    {
        if (needWinRefresh)
        {
            SetWinTeam();
            needWinRefresh = false;
        }
    }

    public void SetWinUserData(List<UserVO> list, int gameOverCase)
    {
        lock (lockObj)
        {
            needWinRefresh = true;
            gameOverUserList = list;
            this.gameOverCase = (GameOverCase)gameOverCase;
        }
    }
    public void SetWinTeam()
    {
        //이긴 팀에 따라 해줘야 할 일 해주기
        Init();
        print("GameOver실행");
        foreach (UserVO uv in gameOverUserList)
        {
            if (uv.socketId == socketId)
            {
                GameOverPanel.Instance.MakeWinImg(user, gameOverCase == GameOverCase.KillAllCitizen);
            }
            else
            {
                Player p = null;
                playerList.TryGetValue(uv.socketId, out p);

                if (p != null)
                {
                    GameOverPanel.Instance.MakeWinImg(p, gameOverCase == GameOverCase.KillAllCitizen);
                }
                
            }
        }
        EventManager.OccurGameOver(gameOverCase);

        foreach (UserVO uv in gameOverUserList)
        {
            if (uv.socketId == socketId)
            {
                user.transform.position = uv.position;
            }
            else
            {
                Player p = null;

                playerList.TryGetValue(uv.socketId, out p);

                if (p != null)
                {
                    p.transform.position = uv.position;
                }
            }
        }
    }
}
