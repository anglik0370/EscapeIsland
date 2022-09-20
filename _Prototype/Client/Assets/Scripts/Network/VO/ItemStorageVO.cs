[System.Serializable]
public class ItemStorageVO : VO
{
    public Team team;
    public int itemSOId;
    public bool isFull;

    public ItemStorageVO()
    {

    }

    public ItemStorageVO(Team team, int itemSOId)
    {
        this.team = team;
        this.itemSOId = itemSOId;
    }
}
