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

    public float lightGlobalIntensity;
    public float lightPointIntensity;
    public float darkGlobalIntensity;
    public float darkPointIntensity;

    [SerializeField]
    private float duration = 1f;
    
    public void Dark()
    {
        DOTween.To(() => global.intensity, x => global.intensity = x, darkGlobalIntensity, duration);
        DOTween.To(() => point.intensity, x => point.intensity = x, darkPointIntensity, duration);
    }

    public void Light()
    {
        DOTween.To(() => global.intensity, x => global.intensity = x, lightGlobalIntensity, duration);
        DOTween.To(() => point.intensity, x => point.intensity = x, lightPointIntensity, duration);
    }
}
