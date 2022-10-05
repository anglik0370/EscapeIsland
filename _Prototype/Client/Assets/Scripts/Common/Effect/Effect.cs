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

    public void SetLocalPosition(Vector2 pos, Player p = null)
    {
        transform.localPosition = pos + offset;

        if(p != null)
        {
            transform.localPosition += p.curSO.adjsutPos;
        }
    }

    public void Play()
    {
        ps.Play();
        Invoke("Disable", ps.main.duration);
    }

    public void LoopingPlay()
    {
        ps.Play();
    }

    public void Disable()
    {
        if (ps.isPlaying)
            ps.Stop();
        gameObject.SetActive(false);
    }
}
