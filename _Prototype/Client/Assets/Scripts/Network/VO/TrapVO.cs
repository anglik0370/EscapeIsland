using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrapVO
{
    public int socketId;
    public int trapId;

    public TrapVO()
    {

    }
    public TrapVO(int socketId,int trapId)
    {
        this.socketId = socketId;
        this.trapId = trapId;
    }
}
