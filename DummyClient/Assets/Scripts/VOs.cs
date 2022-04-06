using System;

[Serializable]
public class UserVO
{
    public string name;
    public int socketId;
    public int roomNum;

    public UserVO()
    {

    }

    public UserVO(string name, int socketId, int roomNum)
    {
        this.name = name;
        this.socketId = socketId;
        this.roomNum = roomNum;
    }
}

[Serializable]
public class LoginVO
{
    public string name;

    public LoginVO()
    {

    }

    public LoginVO(string name)
    {
        this.name = name;
    }
}

[Serializable]
public class RoomVO
{
    public string name;
    public int roomNum;
    public int curUserNum;
    public int userNum;
    public int kidnapperNum;

    public RoomVO()
    {

    }

    public RoomVO(string name, int roomNum, int curUserNum, int userNum, int kidnapperNum)
    {
        this.name = name;
        this.roomNum = roomNum;
        this.curUserNum = curUserNum;
        this.userNum = userNum;
        this.kidnapperNum = kidnapperNum;
    }
}

[Serializable]
public class KillVO
{
    public int targetSocketId;

    public KillVO()
    {

    }

    public KillVO(int targetSocketId)
    {
        this.targetSocketId = targetSocketId;
    }
}
