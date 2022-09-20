using UnityEngine;

[System.Serializable]
public class UserVO : VO
{
    public int socketId;
    public int charId;
    public string name;
    public int roomNum;
    public Vector2 position;
    public bool master;
    public bool isImposter;
    public bool isDie;
    public bool ready;
    public Area area;
    public Team curTeam;
    public float[] voiceData;

    public UserVO()
    {

    }
}
