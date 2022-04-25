[System.Serializable]
public class RefineryVO
{
    public int refineryId;
    public int itemSOId;

    public RefineryVO()
    {

    }

    public RefineryVO(int refineryId,int itemSOId)
    {
        this.refineryId = refineryId;
        this.itemSOId = itemSOId;
    }
}
