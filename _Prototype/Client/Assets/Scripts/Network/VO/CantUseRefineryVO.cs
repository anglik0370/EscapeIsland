[System.Serializable]
public class CantUseRefineryVO
{
    public int refineryId;
    public int slotIdx;

    public CantUseRefineryVO()
    {

    }

    public CantUseRefineryVO(int refineryId, int slotIdx)
    {
        this.refineryId = refineryId;
        this.slotIdx = slotIdx;
    }
}
