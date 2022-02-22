using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAccent : MonoBehaviour
{
    private readonly Color invisibleColor = new Color(1, 1, 1, 0);
    private readonly Color visibleColor = new Color(1, 1, 1, 1);

    private SpriteRenderer sr;

    public Color outlineColor = Color.white;
    public float outlineThickness = 4f;

    private void Awake() 
    {
        sr = GetComponent<SpriteRenderer>();

        Disable();
    }
    
    private void Start() 
    {
        sr.material.SetColor("_OutlineColor", outlineColor);
        sr.material.SetFloat("_OutlineThickness", outlineThickness);
    }

    public void Enable(Sprite sprite, Transform trm, bool isFlip = false)
    {
        sr.color = visibleColor;
        sr.sprite = sprite;

        sr.flipX = isFlip;

        transform.position = trm.position;
        transform.localScale = trm.localScale;
        transform.rotation = trm.rotation;
    }

    public void Disable()
    {
        sr.color = invisibleColor;
        sr.sprite = null;
    }
}
