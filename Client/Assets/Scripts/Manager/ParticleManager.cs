using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }

    [SerializeField]
    private BloodEffect bloodEffectPrefab;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        PoolManager.CreatePool<BloodEffect>(bloodEffectPrefab.gameObject, transform, 5);
    }

    public void PlayBloodEffect(Vector3 position)
    {
        BloodEffect effect = PoolManager.GetItem<BloodEffect>();
        effect.transform.position = position;
        effect.Play();
    }
}
