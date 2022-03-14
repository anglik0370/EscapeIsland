using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePositionSort : MonoBehaviour
{
    [SerializeField]
    private bool bRunOnce = true;

    [SerializeField]
    private float posOffsetY;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        posOffsetY = -transform.localPosition.y;
    }

    private void LateUpdate()
    {
        float precisionMultiplier = 5f;
        spriteRenderer.sortingOrder = (int)(-(transform.position.y + posOffsetY) * precisionMultiplier);

        if(bRunOnce)
        {
            Destroy(this);
        }
    }
}
