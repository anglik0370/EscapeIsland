using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public class LightHandler : MonoBehaviour
{
    [SerializeField]
    private Light2D global;
    [SerializeField]
    private Light2D point;

    private const float DURATION = 1f;
    
    public void Dark()
    {
        DOTween.To(() => global.intensity, x => global.intensity = x, 0.1f, DURATION);
        DOTween.To(() => point.intensity, x => point.intensity = x, 0.5f, DURATION);
    }

    public void Light()
    {
        DOTween.To(() => global.intensity, x => global.intensity = x, 1f, DURATION);
        DOTween.To(() => point.intensity, x => point.intensity = x, 0f, DURATION);
    }
}
