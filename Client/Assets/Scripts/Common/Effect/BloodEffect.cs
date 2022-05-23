using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        ps.Play();
        Invoke("Disable", ps.main.duration);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
