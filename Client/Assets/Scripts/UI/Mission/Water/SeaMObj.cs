using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SeaMObj : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private MissionWater missionWater;

    private Coroutine co;

    [SerializeField]
    private float maxProgress = 5f;
    [SerializeField]
    private float curProgress = 0f;

    [SerializeField]
    private BottleGhostMObj ghost;

    private void Awake()
    {
        missionWater = GetComponentInParent<MissionWater>();
    }

    public void Init()
    {
        if (co != null)
        {
            StopCoroutine(co);
        }

        curProgress = 0f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            if(co != null)
            {
                StopCoroutine(co);
            }

            co = StartCoroutine(PutWaterRoutine());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //�����Ͱ� ȭ�� ������ ������ �� ���� ��

        if (co != null)
        {
            StopCoroutine(co);
        }
    }

    private IEnumerator PutWaterRoutine()
    {
        bool isTouching = true;
        bool isFull = false;

        while(isTouching && !isFull)
        {
            if(Input.GetMouseButtonUp(0))
            {
                print("�׿��� �־����� Ż�� �����");
                isTouching = false;
            }

            if(curProgress >= maxProgress)
            {
                print("�׿��� �־����� �հ� �����");
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
