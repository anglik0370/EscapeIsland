using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArsonManager : MonoBehaviour
{
    public static ArsonManager Instance { get; private set; }

    [SerializeField]
    private List<ItemSO> waterList;

    [SerializeField]
    private List<ArsonSlot> arsonList;
    public List<ArsonSlot> ArsonList => arsonList;

    [SerializeField]
    private Transform arsonParent;

    private SabotageDataVO data;
    public SabotageDataVO Data
    {
        get => data;
        set => data = value;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        arsonList = arsonParent.GetComponentsInChildren<ArsonSlot>().ToList();

        SlotActive(false);
    }

    public void SlotActive(bool active)
    {
        for (int i = 0; i < arsonList.Count; i++)
        {
            arsonList[i].gameObject.SetActive(active);
        }
    }

    public void StartArson()
    {
        arsonList[0].gameObject.SetActive(true); // น่
        arsonList[data.arsonId].gameObject.SetActive(true);
    }

    public bool CanExtinguish(ItemSO so)
    {
        return waterList.Find(s => s == so) != null;
    }

    public bool AllExtinguish(int slotId)
    {
        int cnt = 0;
        bool equalSlot = false;

        for (int i = 0; i < arsonList.Count; i++)
        {
            if(arsonList[i].gameObject.activeSelf)
            {
                cnt++;

                if (arsonList[i].id == slotId) equalSlot = true;
            }
        }

        return cnt == 1 && equalSlot;
    }

    public ArsonSlot GetArsonSlot(int id)
    {
        return arsonList.Find(slot => slot.id == id);
    }

    public int GetRandomSmelter()
    {
        return UnityEngine.Random.Range(1, ArsonList.Count);
    }
}
