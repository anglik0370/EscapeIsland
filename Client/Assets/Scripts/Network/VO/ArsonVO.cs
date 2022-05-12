[System.Serializable]
public class ArsonVO
{
    public int arsonId;
    public bool allExtinguish;

    public ArsonVO()
    {

    }

    public ArsonVO(int arsonId, bool allExtinguish)
    {
        this.arsonId = arsonId;
        this.allExtinguish = allExtinguish;
    }
}
