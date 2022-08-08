using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance { get; private set; }

    private List<BuffSO> buffList;

    private int minAltarId = 200;
    private int maxAltarId = 200;

    private void Awake()
    {
        buffList = Resources.LoadAll<BuffSO>("BuffSO").ToList();

        for (int i = 0; i < buffList.Count; i++)
        {
            BuffSO buff = buffList[i];
            if (buff.id >= 200)
            {
                maxAltarId = Mathf.Max(maxAltarId, buff.id);
            }
        }
        maxAltarId++;

        Instance = this;
    }

    public BuffSO GetBuffSO(int id)
    {
        return buffList.Find(buff => buff.id.Equals(id));
    }

    public int GetAltarBuffId()
    {
        return Random.Range(minAltarId, maxAltarId);
    }
}
