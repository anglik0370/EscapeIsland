using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.Rendering.Universal;

public class LabDoor : MonoBehaviour
{
    [Header("원래 위치를 위한 벡터")]
    private Vector3 defaultPos;
    private Vector3 defaultScale;

    [Header("닫을 때를 위한 Trm")]
    [SerializeField]
    private Transform closeTrm;

    [SerializeField]
    private ShadowCaster2D shadowCaster;

    private float lerpSpeed = 1f;

    private void Awake()
    {
        defaultPos = transform.localPosition;
        defaultScale = transform.localScale;
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
        transform.DOScale(closeTrm.localScale, lerpSpeed);

        yield return CoroutineHandler.fifteenSec;

        Open();
    }

    private void Open()
    {
        transform.DOLocalMove(defaultPos, lerpSpeed);
        transform.DOScale(defaultScale, lerpSpeed);
    }
}
