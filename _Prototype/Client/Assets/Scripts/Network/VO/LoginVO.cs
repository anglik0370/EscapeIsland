[System.Serializable]
public class LoginVO
{
    public string name;
    public int socketId;

    public LoginVO()
    {

    }

    public LoginVO(string name, int socketId)
    {
        this.name = name;
        this.socketId = socketId;
    }
}
