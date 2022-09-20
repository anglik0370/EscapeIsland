using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameOverVO : VO
{
    public List<UserVO> dataList;
    public GameOverCase gameOverCase;

    public GameOverVO()
    {

    }

    public GameOverVO(List<UserVO> list,GameOverCase gameOverCase)
    {
        dataList = list;
        this.gameOverCase = gameOverCase;
    }
}
