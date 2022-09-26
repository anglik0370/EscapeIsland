using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }

    //[SerializeField]
    //private Effect faintEffectPrefab;
    //[SerializeField]
    //private Effect flyPaperEffectPrefab;
    //[SerializeField]
    //private Effect dissRapEffectPrefab;

    [SerializeField]
    private List<Effect> effectPrefabList = new List<Effect>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        //PoolManager.CreatePool<Effect>(faintEffectPrefab.gameObject, transform,"faint", 5);

        for (int i = 0; i < effectPrefabList.Count; i++)
        {
            PoolManager.CreatePool<Effect>(effectPrefabList[i].gameObject, transform, effectPrefabList[i].Key, 5);
        }
    }

    public void PlayEffect(string key, Vector3 pos)
    {
        Effect effect = PoolManager.GetItem<Effect>(key);
        effect.transform.SetParent(null);
        effect.SetPosition(pos);
        effect.Play();
    }

    public void PlayEffect(string key, Transform parent)
    {
        Effect effect = PoolManager.GetItem<Effect>(key);
        effect.transform.SetParent(parent);
        effect.SetLocalPosition(Vector2.zero);
        effect.Play();
    }
}
