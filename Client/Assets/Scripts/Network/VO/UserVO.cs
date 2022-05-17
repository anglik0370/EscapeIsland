using UnityEngine;

[System.Serializable]
public class UserVO
{
    public int socketId;
    public int charId;
    public string name;
    public int roomNum;
    public Vector2 position;
    public bool master;
    public bool isImposter;
    public bool isDie;
    public AreaState areaState;

    public UserVO()
    {

    }

    public UserVO(int socketId,int charId, string name, int roomNum, Vector2 pos, bool master, bool isImposter,bool isDie,AreaState areaState)
    {
        this.socketId = socketId;
        this.charId = charId;
        this.name = name;
        this.roomNum = roomNum;
        this.position = pos;
        this.master = master;
        this.isImposter = isImposter;
        this.isDie = isDie;
        this.areaState = areaState;
    }
}
