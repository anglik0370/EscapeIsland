using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TwinkleImg : MonoBehaviour
{
    private Image img;

    private Coroutine co;
    private Sequence seq;

    [SerializeField]
    private float twinkleDuration;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    public void StartTwinkle()
    {
        StopTwinkle();

        img.color = UtilClass.opacityColor;

        co = StartCoroutine(TwinkleRoutine());
    }

    public void StopTwinkle()
    {
        if (co != null)
        {
            StopCoroutine(co);
        }

        if (seq != null)
        {
            seq.Kill();
        }

        img.color = UtilClass.limpidityColor;
    }

    public IEnumerator TwinkleRoutine()
    {
        while (true)
        {
            if (seq != null)
            {
                seq.Kill();
            }

            seq = DOTween.Sequence();

            seq.Append(img.DOColor(UtilClass.limpidityColor, twinkleDuration / 2));
            seq.Append(img.DOColor(UtilClass.opacityColor, twinkleDuration / 2));

            yield return new WaitForSeconds(seq.Duration());
        }
    }
}
