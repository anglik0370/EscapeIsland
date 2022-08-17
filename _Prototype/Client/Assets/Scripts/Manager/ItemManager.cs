using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    [SerializeField]
    private List<ItemSO> itemList = new List<ItemSO>();
    public List<ItemSO> ItemList => itemList;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        itemList = Resources.LoadAll<ItemSO>("ItemSO/").ToList();
    }

    public ItemSO FindItemSO(int id)
    {
        return itemList.Find(x => x.itemId == id);
    }

    public ItemAmount FindItemAmount(List<ItemAmount> list, ItemSO item)
    {
        ItemAmount amount = list.Find(x => x.item.itemId == item.itemId);

        return amount;
    }
}
