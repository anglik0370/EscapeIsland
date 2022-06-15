using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameOverVO
{
    public List<UserVO> dataList;
    public int gameOverCase;

    public GameOverVO()
    {

    }

    public GameOverVO(List<UserVO> list,int gameOverCase)
    {
        dataList = list;
        this.gameOverCase = gameOverCase;
    }
}
