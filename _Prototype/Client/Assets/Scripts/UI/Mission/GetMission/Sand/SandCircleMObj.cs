using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SandCircleMObj : MonoBehaviour, IPointerClickHandler
{
    private CanvasGroup cvs;
    private RectTransform smallCircle;
    private RectTransform bigCircle;

    [Header("큰 원의 크기값")]
    [SerializeField]
    private float maxScale;
    [SerializeField]
    private float minScale;

    [Header("판정")]
    [SerializeField]
    private float perfectScale;
    [SerializeField]
    private float goodCorrectionValue;
    [SerializeField]
    private float correctionValue;

    [Header("큰 원의 크기 감소량")]
    [SerializeField]
    private float decrease;

    [SerializeField]
    private bool isTouch = false;
    public bool IsTouch => isTouch;

    private Action<bool> ClickSuccess = isPerfect => { };
    private Action ClickFail = () => { };

    private Coroutine co;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
        smallCircle = GetComponent<RectTransform>();
        bigCircle = GetComponentsInChildren<RectTransform>()[1];
    }

    public void Init()
    {
        isTouch = false;

        UtilClass.SetCanvasGroup(cvs, 1, true, true);

        bigCircle.localScale = new Vector3(maxScale, maxScale);
    }

    public void Disable()
    {
        UtilClass.SetCanvasGroup(cvs);

        if (co != null)
        {
            StopCoroutine(co);
        }

    }

    public void OccurRoutineComplete(Action<bool> SuccessCallback, Action FailCallback)
    {
        ClickSuccess += SuccessCallback;
        ClickFail += FailCallback;
    }

    public void SetPosition(Vector2 pos)
    {
        smallCircle.anchoredPosition = pos;
    }

    public void StartShrink()
    {
        if(co != null)
        {
            StopCoroutine(co);
        }

        co = StartCoroutine(ShrinkCircleRoutine());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTouch) return;

        isTouch = true;
    }

    public IEnumerator ShrinkCircleRoutine()
    {
        Vector3 scale = bigCircle.localScale;

        while(bigCircle.localScale.x > minScale)
        {
            if(isTouch)
            {
                if (bigCircle.localScale.x >= perfectScale - correctionValue
                    && bigCircle.localScale.x <= perfectScale + correctionValue)
                {
                    ClickSuccess?.Invoke(true);
                }
                else if (bigCircle.localScale.x >= perfectScale - goodCorrectionValue
                    && bigCircle.localScale.x <= perfectScale + goodCorrectionValue)
                {
                    ClickSuccess?.Invoke(false);
                }
                else
                {
                    ClickFail?.Invoke();
                }

                UtilClass.SetCanvasGroup(cvs);
                yield break;
            }

            scale = new Vector3(scale.x - decrease, scale.y - decrease);
            bigCircle.localScale = scale;

            yield return null;
        }

        UtilClass.SetCanvasGroup(cvs);
        ClickFail?.Invoke();
    }
}
