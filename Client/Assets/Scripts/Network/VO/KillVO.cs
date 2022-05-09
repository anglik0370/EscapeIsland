[System.Serializable]
public class KillVO
{
    public int killerId;
    public int targetSocketId;

    public KillVO()
    {

    }

    public KillVO(int killerId, int targetSocketId)
    {
        this.killerId = killerId;
        this.targetSocketId = targetSocketId;
    }
}
