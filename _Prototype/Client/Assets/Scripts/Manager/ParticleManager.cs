using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }

    [SerializeField]
    private List<Effect> effectPrefabList = new List<Effect>();

    [SerializeField]
    private ParticleDisplayer displayer;

    [SerializeField]
    private Camera effectCam;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        for (int i = 0; i < effectPrefabList.Count; i++)
        {
            PoolManager.CreatePool<Effect>(effectPrefabList[i].gameObject, transform, effectPrefabList[i].Key, 5);
        }
    }

    #region OverlayEffect
    public void PlayEffectScreenToWorldPoint(string key, Vector3 pos)
    {
        //pos = Camera.main.ScreenToWorldPoint(pos);
        Effect effect = PoolManager.GetItem<Effect>(key);

        effect.transform.SetParent(null);
        effect.SetPosition(pos);
        displayer.MoveToPosition(pos);

        effect.Play();
    }
    #endregion

    public void PlayEffect(string key, Vector3 pos)
    {
        Effect effect = PoolManager.GetItem<Effect>(key);
        effect.transform.SetParent(null);
        effect.SetPosition(pos);
        displayer.MoveToPosition(pos);
        effect.Play();
    }

    public void PlayEffect(string key, Transform parent)
    {
        Effect effect = PoolManager.GetItem<Effect>(key);
        effect.transform.SetParent(parent);
        effect.SetLocalPosition(Vector2.zero);
        displayer.MoveToPosition(effect.transform.position);
        effect.Play();
    }
}
