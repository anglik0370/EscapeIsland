using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.Rendering.Universal;

public class LabDoor : MonoBehaviour
{
    [Header("���� ��ġ�� ���� ����")]
    private Vector3 defaultPos;
    private Vector3 defaultScale;

    [Header("���� ���� ���� Trm")]
    [SerializeField]
    private Transform closeTrm;

    [SerializeField]
    private ShadowCaster2D shadowCaster;

    [SerializeField]
    private SpriteRenderer sr;

    private float lerpSpeed = 1f;

    private void Awake()
    {
        defaultPos = transform.localPosition;
        defaultScale = transform.localScale;

        shadowCaster.enabled = false;

        //sr.size.y
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CloseDoor();
        }
    }

    public void CloseDoor()
    {
        StartCoroutine(Close());
    }

    private IEnumerator Close()
    {
        transform.DOLocalMove(closeTrm.localPosition, lerpSpeed);
        transform.DOScale(closeTrm.localScale, lerpSpeed).OnComplete(() =>
        {
            shadowCaster.enabled = true;
        });

        yield return CoroutineHandler.fifteenSec;

        Open();
    }

    private void Open()
    {
        transform.DOLocalMove(defaultPos, lerpSpeed);
        transform.DOScale(defaultScale, lerpSpeed).OnComplete(() =>
        {
            shadowCaster.enabled = false;
        });
    }
}
