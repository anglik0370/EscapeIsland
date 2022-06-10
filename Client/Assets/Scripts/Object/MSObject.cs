using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSObject : MonoBehaviour
{
    [SerializeField]
    private bool isEmpty;
    public bool IsEmpty => isEmpty;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Enable()
    {
        isEmpty = false;
        sr.color = UtilClass.opacityColor;
    }

    public void Disable()
    {
        isEmpty = true;
        sr.color = UtilClass.limpidityColor;
    }
}
