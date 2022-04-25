using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public class LightHandler : MonoBehaviour
{
    public static LightHandler Instance { get; private set; }

    [Header("�����")]
    [SerializeField]
    private Light2D global;
    [SerializeField]
    private Light2D shadowPoint;
    [SerializeField]
    private Light2D lightMapPoint;
    [SerializeField]
    private Light2D refineryPoint;
    [SerializeField]
    private Light2D labotoryPoint;

    [Header("�����Ҷ� �������ϴ°͵�")]
    [SerializeField]
    private GameObject[] lightMapObjs;

    [Header("GlobalLight ���")]
    public float lightGlobalIntensity;
    public float darkGlobalIntensity;

    [Header("Player PointLight ���")]
    public float lightPointIntensity;
    public float darkPointIntensity;

    [Header("Player PointLight ���� ������ ũ��")]
    public float lightInnerRadius;
    public float darkInnerRadius;

    [Header("Player PointLight �ٱ��� ������ ũ��")]
    public float lightOuterRadius;
    public float darkOuterRadius;

    [Header("�ǳ� ���� ���")]
    public float lightInsideLightIntensity;
    public float darkInsideLightIntensity;

    [Header("��� ��ȭ �ð�")]
    [SerializeField]
    private float duration = 1f;
    
    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        for(int i = 0; i < lightMapObjs.Length; i++)
        {
            lightMapObjs[i].SetActive(true);
        }

        EventManager.SubTimeChange(isLight =>
        {
            if(isLight)
            {
                Light();
            }
            else
            {
                Dark();
            }
        });

        EventManager.SubEnterRoom(p =>
        {
            Light2D[] lights = p.GetComponentsInChildren<Light2D>();

            lightMapPoint = lights[0];
            shadowPoint = lights[1];
        });
    }

    public void Dark()
    {
        DOTween.To(() => global.intensity, x => global.intensity = x, darkGlobalIntensity, duration);
        DOTween.To(() => shadowPoint.intensity, x => shadowPoint.intensity = x, darkPointIntensity, duration);

        DOTween.To(() => refineryPoint.intensity, x => refineryPoint.intensity = x, darkInsideLightIntensity, duration);
        DOTween.To(() => labotoryPoint.intensity, x => labotoryPoint.intensity = x, darkInsideLightIntensity, duration);

        DOTween.To(() => shadowPoint.pointLightInnerRadius, x => shadowPoint.pointLightInnerRadius = x, darkInnerRadius, duration);
        DOTween.To(() => shadowPoint.pointLightOuterRadius, x => shadowPoint.pointLightOuterRadius = x, darkOuterRadius, duration);
        DOTween.To(() => lightMapPoint.pointLightInnerRadius, x => lightMapPoint.pointLightInnerRadius = x, darkInnerRadius, duration);
        DOTween.To(() => lightMapPoint.pointLightOuterRadius, x => lightMapPoint.pointLightOuterRadius = x, darkOuterRadius, duration);
    }

    public void Light()
    {
        DOTween.To(() => global.intensity, x => global.intensity = x, lightGlobalIntensity, duration);
        DOTween.To(() => shadowPoint.intensity, x => shadowPoint.intensity = x, lightPointIntensity, duration);

        DOTween.To(() => refineryPoint.intensity, x => refineryPoint.intensity = x, lightInsideLightIntensity, duration);
        DOTween.To(() => labotoryPoint.intensity, x => labotoryPoint.intensity = x, lightInsideLightIntensity, duration);

        DOTween.To(() => shadowPoint.pointLightInnerRadius, x => shadowPoint.pointLightInnerRadius = x, lightInnerRadius, duration);
        DOTween.To(() => shadowPoint.pointLightOuterRadius, x => shadowPoint.pointLightOuterRadius = x, lightOuterRadius, duration);
        DOTween.To(() => lightMapPoint.pointLightInnerRadius, x => lightMapPoint.pointLightInnerRadius = x, lightInnerRadius, duration);
        DOTween.To(() => lightMapPoint.pointLightOuterRadius, x => lightMapPoint.pointLightOuterRadius = x, lightOuterRadius, duration);
    }
}
