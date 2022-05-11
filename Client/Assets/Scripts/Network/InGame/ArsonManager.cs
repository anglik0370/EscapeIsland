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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        arsonList = arsonParent.GetComponentsInChildren<ArsonSlot>().ToList();
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
        //arsonList[idx].gameObject.SetActive(true);
    }

    public bool CanExtinguish(ItemSO so)
    {
        return waterList.Find(s => s == so) != null;
    }

    public bool AllExtinguish()
    {
        return arsonList.Find(slot => slot.gameObject.activeSelf) == null;
    }

    public ArsonSlot GetArsonSlot(int id)
    {
        return arsonList.Find(slot => slot.id == id);
    }

}
