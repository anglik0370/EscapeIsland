using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePositionSort : MonoBehaviour
{
    [SerializeField]
    private bool bRunOnce = true;

    [SerializeField]
    private Transform otherTrm;

    [SerializeField]
    private bool useOtherTrm = false;

    [SerializeField]
    private bool useReverse = false;

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
        float precisionMultiplier = 10f;
        //spriteRenderer.sortingOrder = (int)(-(transform.position.y + posOffsetY) * precisionMultiplier);

        if (useReverse) precisionMultiplier *= -1;

        if(useOtherTrm)
        {
            spriteRenderer.sortingOrder = (int)((0 - otherTrm.position.y) * precisionMultiplier);
        }
        else
        {
            spriteRenderer.sortingOrder = (int)((0 - transform.position.y) * precisionMultiplier);
        }

        if(bRunOnce)
        {
            Destroy(this);
        }
    }
}
