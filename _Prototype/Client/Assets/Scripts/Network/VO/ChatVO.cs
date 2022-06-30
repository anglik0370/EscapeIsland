[System.Serializable]
public class ChatVO
{
    public int socketId;
    public string msg;
    public ChatType chatType;

    public ChatVO()
    {

    }

    public ChatVO(int socketId,string msg,ChatType chatType)
    {
        this.socketId = socketId;
        this.msg = msg;
        this.chatType = chatType;
    }
}
