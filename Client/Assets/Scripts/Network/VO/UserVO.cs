using UnityEngine;

[System.Serializable]
public class UserVO
{
    public int socketId;
    public string name;
    public int roomNum;
    public Vector2 position;
    public bool master;

    public UserVO()
    {

    }

    public UserVO(int socketId, string name, int roomNum, Vector2 pos, bool master)
    {
        this.socketId = socketId;
        this.name = name;
        this.roomNum = roomNum;
        this.position = pos;
        this.master = master;
    }
}
