using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrapVO : VO
{
    public int socketId;
    public int id;

    public TrapVO()
    {

    }
    public TrapVO(int socketId,int trapId)
    {
        this.socketId = socketId;
        this.id = trapId;
    }
}

[System.Serializable]
public class FlyPaperVO : VO
{
    public int socketId;
    public int id;
    public int userId;

    public FlyPaperVO()
    {

    }
    public FlyPaperVO(int socketId, int trapId, int userId)
    {
        this.socketId = socketId;
        this.id = trapId;
        this.userId = userId;
    }
}
