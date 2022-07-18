using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaStateHolder : MonoBehaviour
{
    [SerializeField]
    private Area area;
    public Area Area => area;

    private SpriteRenderer sr;
    public SpriteRenderer Sr => sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
}
