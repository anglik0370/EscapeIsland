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
            //��ȣ�ۿ���� �ȿ� �ִ��� üũ
            if (Vector2.Distance(player.GetTrm().position, converterList[i].GetInteractionTrm().position) <= player.range)
            {
                if (converter == null)
                {
                    //������ �ϳ� �־��ְ�
                    converter = converterList[i];
                }
                else
                {
                    //������ �Ÿ���
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
