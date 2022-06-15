using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaStateHolder : MonoBehaviour
{
    [SerializeField]
    private AreaState areaState;
    public AreaState AreaState => areaState;

    private SpriteRenderer sr;
    public SpriteRenderer Sr => sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
}
