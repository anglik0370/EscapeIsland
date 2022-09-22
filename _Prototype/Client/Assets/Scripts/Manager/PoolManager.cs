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

    /// <summary>
    /// 이펙트 용으로 만듦
    /// </summary>
    /// <param name="key">문자열 키</param>=
    public static void CreatePool<T>(GameObject prefab, Transform parent, string key, int count = 5) where T : MonoBehaviour
    {
        ObjectPool<T> pool = new ObjectPool<T>(prefab, parent, count);
        poolDic.Add(key, pool);
    }

    /// <summary>
    /// 이펙트 용으로 만듦
    /// </summary>
    /// <param name="key">문자열 키</param>
    public static T GetItem<T>(string key) where T : MonoBehaviour
    {
        ObjectPool<T> pool = (ObjectPool<T>)poolDic[key];
        return pool.GetOrCreate();
    }
}
