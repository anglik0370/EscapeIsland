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
        });
    }

    public bool FindProximateConverter(out ItemConverter temp)
    {
        ItemConverter converter = null;

        for (int i = 0; i < converterList.Count; i++)
        {
            //상호작용범위 안에 있는지 체크
            if (Vector2.Distance(player.GetTrm().position, converterList[i].GetInteractionTrm().position) <= player.range)
            {
                if (converter == null)
                {
                    //없으면 하나 넣어주고
                    converter = converterList[i];
                }
                else
                {
                    //있으면 거리비교
                    if (Vector2.Distance(player.GetTrm().position, converter.GetTrm().position) >
                        Vector2.Distance(player.GetTrm().position, converterList[i].GetTrm().position))
                    {
                        converter = converterList[i];
                    }
                }
            }
        }

        temp = converter;

        return temp != null;
    }
}
