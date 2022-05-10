[System.Serializable]
public class CantUseRefineryVO
{
    public int refineryId;
    public bool canUseRefinery;
    public int slotIdx;

    public CantUseRefineryVO()
    {

    }

    public CantUseRefineryVO(int refineryId, bool canUseRefinery, int slotIdx)
    {
        this.refineryId = refineryId;
        this.canUseRefinery = canUseRefinery;
        this.slotIdx = slotIdx;
    }
}
