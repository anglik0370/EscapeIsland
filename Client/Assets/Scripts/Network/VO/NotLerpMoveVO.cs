using UnityEngine;

[System.Serializable]
public class NotLerpMoveVO
{
    public int socketId;
    public Vector2 pos;

    public NotLerpMoveVO()
    {

    }

    public NotLerpMoveVO(int socketId,Vector2 pos)
    {
        this.socketId = socketId;
        this.pos = pos;
    }
}
