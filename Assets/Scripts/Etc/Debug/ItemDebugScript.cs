using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDebugScript : MonoBehaviour
{
    public List<ItemSO> itemList;

    private void Awake() 
    {
        itemList = Resources.LoadAll<ItemSO>(typeof(ItemSO).ToString()).ToList();

        itemList.Sort((x, y) => x.itemId - y.itemId);
        itemList.ForEach(x => print(x.itemName));    
    }
}
