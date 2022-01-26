using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refinery : MonoBehaviour
{
    public int id;

    public ItemSO oreItem;
    public ItemSO ingotItem;

    private Dictionary<ItemSO, ItemSO> refiningDic;

    private string orePathFormat = "ItemSO/Item{0}Ore";
    private string ingotPathFormat = "ItemSO/Item{0}Ingot";

    private string[] oreNames = new string[]
    {
        "Copper", "Iron", "Sand"
    };

    public bool isRefiningEnd = true; //재련이 끝났는지

    public float refiningTime = 5f; //재련하는데 걸리는 시간
    public float remainTime; //남은 재련시간

    private void Awake() 
    {
        refiningDic = new Dictionary<ItemSO, ItemSO>();

        for(int i = 0; i < oreNames.Length; i++)
        {
            ItemSO ore = Resources.Load<ItemSO>(string.Format(orePathFormat, oreNames[i]));
            ItemSO ingot = Resources.Load<ItemSO>(string.Format(ingotPathFormat, oreNames[i]));

            refiningDic.Add(ore, ingot);
        }
    }

    private void Update() 
    {
        if(!isRefiningEnd)
        {
            remainTime -= Time.deltaTime;

            if(remainTime <= 0)
            {
                EndRefining();
            }
        }
    }

    public void StartRefining(ItemSO oreItem)
    {
        //제련 시작 시 (제련 전 아이템을 넣었을 때)
        //필요한것 - 제련소 id, 넣은 아이템SO
        isRefiningEnd = false;
        remainTime = refiningTime;

        this.oreItem = oreItem;

        if(RefineryPanel.Instance.IsOpenRefinery(this))
        {
            RefineryPanel.Instance.SetNameText(oreItem.ToString(), FindIngotFromOre(oreItem).ToString());
        }
    }

    public void ResetRefining()
    {
        //제련이 다 되기 전에 뺐을때
        remainTime = 0f;
        isRefiningEnd = true;

        if(RefineryPanel.Instance.IsOpenRefinery(this))
        {
            RefineryPanel.Instance.UpdateImg();
        }
    }

    public void EndRefining()
    {
        remainTime = 0f;
        isRefiningEnd = true;

        //제련 끝나면 해줄일
        ingotItem = FindIngotFromOre(oreItem);
        oreItem = null;

        if(RefineryPanel.Instance.IsOpenRefinery(this))
        {
            RefineryPanel.Instance.UpdateImg();
        }
    }

    public ItemSO FindIngotFromOre(ItemSO oreItem)
    {
        return refiningDic[oreItem];
    }
}
