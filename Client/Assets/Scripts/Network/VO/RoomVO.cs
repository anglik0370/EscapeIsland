[System.Serializable]
public class RoomVO
{
    public string name;
    public int roomNum;
    public int userNum;

    public RoomVO()
    {

    }

    public RoomVO(string name,int roomNum, int userNum)
    {
        this.name = name;
        this.roomNum = roomNum;
        this.userNum = userNum;
    }
}
