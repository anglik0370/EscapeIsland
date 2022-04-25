[System.Serializable]
public class ChatVO
{
    public int socketId;
    public string msg;

    public ChatVO()
    {

    }

    public ChatVO(int socketId,string msg)
    {
        this.socketId = socketId;
        this.msg = msg;
    }
}
