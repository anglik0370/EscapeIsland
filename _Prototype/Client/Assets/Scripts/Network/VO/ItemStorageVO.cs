[System.Serializable]
public class ItemStorageVO
{
    public Team team;
    public int itemSOId;

    public ItemStorageVO()
    {

    }

    public ItemStorageVO(Team team, int itemSOId)
    {
        this.team = team;
        this.itemSOId = itemSOId;
    }
}
