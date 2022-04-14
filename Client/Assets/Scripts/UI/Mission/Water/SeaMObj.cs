using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SeaMObj : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Coroutine co;

    [SerializeField]
    private float maxProgress = 5f;
    [SerializeField]
    private float curProgress = 0f;

    [SerializeField]
    private BottleGhostMObj ghost;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Input.GetMouseButton(0))
        {
            print("여기가 물임");

            if(co != null)
            {
                StopCoroutine(co);
            }

            co = StartCoroutine(PutWaterRoutine());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //포인터가 화면 밖으로 나갔을 때 해줄 일

        if (co != null)
        {
            StopCoroutine(co);
        }

        print("여긴 물이 아닌데?");
    }

    private IEnumerator PutWaterRoutine()
    {
        bool isTouching = true;
        bool isFull = false;

        while(isTouching && !isFull)
        {
            if(Input.GetMouseButtonUp(0))
            {
                print("그에게 주어지는 탈락 목걸이");
                isTouching = false;
            }

            if(curProgress >= maxProgress)
            {
                print("그에게 주어지는 합격 목걸이");
                isFull = true;
            }
            else
            {
                curProgress += Time.deltaTime;
                ghost.SetWaterProgress(curProgress / maxProgress);
            }

            yield return null;
        }
    }
}
