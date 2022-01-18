using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObj : MonoBehaviour
{
    private SpriteRenderer sr;
    
    private void Awake() 
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite)
    {
        sr.sprite = sprite;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
}
