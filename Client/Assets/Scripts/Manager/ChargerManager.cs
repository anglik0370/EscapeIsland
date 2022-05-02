using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChargerManager : MonoBehaviour
{
    public static ChargerManager Instance { get; set; }

    [SerializeField]
    private Transform chargerParentTrm;

    [SerializeField]
    private List<ItemCharger> chargerList;

    private void Awake()
    {
        chargerList = chargerParentTrm.GetComponentsInChildren<ItemCharger>().ToList();

        Instance = this;
    }

    public ItemCharger FindChargerById(int id)
    {
        ItemCharger charger = chargerList.Find(x => x.Id == id);

        if(charger == null)
        {
            print("그런건 없어");
        }

        return charger;
    }    
}
