using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IPool
{

}

public class PoolManager
{
    private static Dictionary<string, IPool> poolDic = new Dictionary<string, IPool>();

    public static void CreatePool<T>(GameObject prefab, Transform parent, int count = 5) where T : MonoBehaviour
    {
        Type t = typeof(T);
        ObjectPool<T> pool = new ObjectPool<T>(prefab, parent, count);
        poolDic.Add(t.ToString(), pool);
    }

    public static T GetItem<T>() where T : MonoBehaviour
    {
        Type t = typeof(T);
        ObjectPool<T> pool = (ObjectPool<T>)poolDic[t.ToString()];
        return pool.GetOrCreate();
    }
}
