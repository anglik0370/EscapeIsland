using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LabDoor : MonoBehaviour
{
    [SerializeField]
    private Vector3 closeScale = Vector3.zero;

    private Vector3 defaultPos = Vector3.zero;

    private Vector3 defaultVec = new Vector3(1, 1, 1);

    private float lerpSpeed = 1f;

    private void Awake()
    {
        defaultPos = transform.localPosition;   
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
        transform.DOLocalMove(Vector3.zero, lerpSpeed);
        transform.DOScale(closeScale, lerpSpeed);
        yield return CoroutineHandler.fifteenSec;

        Open();
    }

    private void Open()
    {
        transform.DOLocalMove(defaultPos, lerpSpeed);
        transform.DOScale(defaultVec, lerpSpeed);
    }
}
