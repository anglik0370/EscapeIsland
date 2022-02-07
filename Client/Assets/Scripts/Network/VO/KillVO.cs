[System.Serializable]
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
