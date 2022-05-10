using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConverterManager : MonoBehaviour
{
    public static ConverterManager Instance { get; private set; }

    private List<ItemConverter> converterList = new List<ItemConverter>();
    public List<ItemConverter> ConverterList => converterList;

    private Player player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        converterList = GameObject.FindObjectsOfType<ItemConverter>().ToList();
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            player = p;

            for (int i = 0; i < converterList.Count; i++)
            {
                GameManager.Instance.AddInteractionObj(converterList[i]);
            }
        });
    }

    public List<ItemConverter> GetRefineryList()
    {
        return converterList.FindAll(converter => converter.IsRefinery);
    }

    public ItemConverter GetRefinery(int id)
    {
        return converterList.Find(converter => converter.IsRefinery && converter.id == id);
    }
}
