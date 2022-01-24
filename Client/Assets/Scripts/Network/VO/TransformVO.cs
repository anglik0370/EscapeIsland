using UnityEngine;

[System.Serializable]
public class TransformVO
{
    public Vector2 position;
    public int socketId;

    public TransformVO()
    {

    }

    public TransformVO(Vector2 pos, int socketId)
    {
        position = pos;
        this.socketId = socketId;
    }
}
