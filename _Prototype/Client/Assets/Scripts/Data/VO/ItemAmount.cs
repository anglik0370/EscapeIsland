[System.Serializable]
public class ItemAmount
{
    public ItemSO item;
    public int amount;

    public ItemAmount(ItemSO item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}
