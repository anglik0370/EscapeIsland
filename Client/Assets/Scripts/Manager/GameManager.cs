using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private Transform playerPrefabParent;
    private List<Player> playerList = new List<Player>();

    public List<ItemSpawner> spawnerList = new List<ItemSpawner>();
    public List<Refinery> refineryList = new List<Refinery>();
    public List<ItemSO> itemList = new List<ItemSO>();
    public List<DeadBody> deadBodyList = new List<DeadBody>();

    public DeadBody deadBodyPrefab;

    private StoragePanel storagePanel;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        itemList = Resources.LoadAll<ItemSO>("ItemSO/").ToList();
        spawnerList = GameObject.FindObjectsOfType<ItemSpawner>().ToList();
        refineryList = GameObject.FindObjectsOfType<Refinery>().ToList();
        storagePanel = FindObjectOfType<StoragePanel>();

        PoolManager.CreatePool<DeadBody>(deadBodyPrefab.gameObject, transform, 5);
    }

    public ItemSO FindItemFromItemId(int id)
    {
        return itemList.Find(x => x.itemId == id);
    }

    public void AddItemInStorage(ItemSO so)
    {
        storagePanel.AddItem(so);

        if(storagePanel.IsItemFull(so))
        {
            Debug.Log($"{so}²ËÂü");
        }

        if (storagePanel.IsItemFull())
        {
            //²ËÃ¡À¸´Ï ²ËÃ¡´Ù°í ¼­¹ö¿¡ º¸³»Áà¾ß ÇÑ´Ù.
            DataVO dataVO = new DataVO("STORAGE_FULL", "");

            Debug.Log("²ËÂü");

            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
        }
    }

    public void ClearDeadBody()
    {
        if (deadBodyList.Count == 0) return;

        foreach (DeadBody deadBody in deadBodyList)
        {
            if (!deadBody.gameObject.activeSelf)
            {
                continue;
            }

            deadBody.gameObject.SetActive(false);
        }

        deadBodyList.Clear();
    }

    public void RefreshPlayerList()
    {
        playerList = playerPrefabParent.GetComponentsInChildren<Player>().ToList();
    }

    public int ActivePlyerAmount()
    {
        int cnt = 0;

        for (int i = 0; i < playerList.Count; i++)
        {
            if(playerList[i].gameObject.activeSelf)
            {
                cnt++;
            }
        }

        return cnt;
    }
}
