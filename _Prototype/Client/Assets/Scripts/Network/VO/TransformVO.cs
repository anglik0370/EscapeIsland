using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransformVO : VO
{
    public Vector2 position;
    public int socketId;
    public List<int> itemList;

    public TransformVO()
    {

    }

    public TransformVO(Vector2 pos, int socketId, List<int> itemList)
    {
        position = pos;
        this.socketId = socketId;
        this.itemList = itemList;
    }
}
