using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance { get; private set; }

    private List<BuffSO> buffList;

    private void Awake()
    {
        buffList = Resources.LoadAll<BuffSO>("BuffSO").ToList();

        Instance = this;
    }

    public BuffSO GetBuffSO(int id)
    {
        return buffList.Find(buff => buff.id.Equals(id));
    }
}
