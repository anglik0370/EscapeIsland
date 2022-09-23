using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private ParticleSystem ps;

    [SerializeField]
    private string key;
    public string Key => key;

    [SerializeField]
    private Vector2 offset;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void SetPosition(Vector2 pos)
    {
        transform.position = pos + offset;
    }

    public void SetLocalPosition(Vector2 pos)
    {
        transform.localPosition = pos + offset;
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
