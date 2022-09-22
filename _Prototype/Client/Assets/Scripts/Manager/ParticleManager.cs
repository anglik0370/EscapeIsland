using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }

    [SerializeField]
    private Effect bloodEffectPrefab;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        PoolManager.CreatePool<Effect>(bloodEffectPrefab.gameObject, transform,"blood", 5);
    }

    public void PlayBloodEffect(Vector3 position)
    {
        Effect effect = PoolManager.GetItem<Effect>("blood");
        effect.SetPosition(position);
        effect.Play();
    }
}
