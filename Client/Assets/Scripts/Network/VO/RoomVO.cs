[System.Serializable]
public class RoomVO
{
    public string name;
    public int roomNum;
    public int curUserNum;
    public int userNum;

    public RoomVO()
    {

    }

    public RoomVO(string name,int roomNum,int curUserNum ,int userNum)
    {
        this.name = name;
        this.roomNum = roomNum;
        this.curUserNum = curUserNum;
        this.userNum = userNum;
    }
}
