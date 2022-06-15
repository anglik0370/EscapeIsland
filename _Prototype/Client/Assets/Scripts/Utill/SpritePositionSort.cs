using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpritePositionSort : MonoBehaviour
{
    [SerializeField]
    private bool bRunOnce = true;
    private bool bInitOnce = false;

    [SerializeField]
    private Transform otherTrm;

    [SerializeField]
    private bool useOtherTrm = false;

    [SerializeField]
    private List<SpriteRenderer> srList;
    private List<int> originOrderList;

    [SerializeField]
    private bool useSrList = false;

    private SpriteRenderer spriteRenderer;

    private void LateUpdate()
    {
        if(!bInitOnce)
        {
            if (useSrList)
            {
                originOrderList = new List<int>();

                for (int i = 0; i < srList.Count; i++)
                {
                    originOrderList.Add(srList[i].sortingOrder);
                }
            }
            else
            {
                spriteRenderer = this.GetComponent<SpriteRenderer>();
            }
            bInitOnce = true;
        }

        float precisionMultiplier = 100f;

        if (useOtherTrm)
        {
            if (useSrList)
            {
                for (int i = 0; i < srList.Count; i++)
                {
                    srList[i].sortingOrder = (int)((0 - otherTrm.position.y) * precisionMultiplier) + originOrderList[i];
                }
            }
            else
            {
                spriteRenderer.sortingOrder = (int)((0 - otherTrm.position.y) * precisionMultiplier);
            }
        }
        else
        {
            if (useSrList)
            {
                Debug.LogWarning("이거 쓸라면 useOtherTrm도 키라고 헀잖아");
            }

            spriteRenderer.sortingOrder = (int)((0 - transform.position.y) * precisionMultiplier);
        }

        if(bRunOnce)
        {
            Destroy(this);
        }
    }
}
