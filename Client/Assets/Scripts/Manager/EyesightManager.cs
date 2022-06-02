using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public class EyesightManager : MonoBehaviour
{
    public static EyesightManager Instance { get; private set; }

    private Player player;

    [SerializeField]
    private Transform objectParentTrm;
    [SerializeField]
    private Transform arsonParentTrm;

    [SerializeField]
    private List<GameObject> otherList;

    private List<AreaStateHolder> areaStateHolderList;

    private List<AreaStateHolder> arsonAreaStateHolderList;
    private List<ArsonSlot> arsonSlotList;

    [Header("쉐도우 캐스터 부모")]
    [SerializeField]
    private GameObject shadowCasterParent;

    [Header("조명들")]
    [SerializeField]
    private Light2D global;
    [SerializeField]
    private Light2D shadowPoint;
    [SerializeField]
    private Light2D lightMapPoint;

    [Header("편집할때 꺼놔야하는것들")]
    [SerializeField]
    private GameObject[] lightMapObjs;

    [Header("GlobalLight 밝기")]
    public float lightGlobalIntensity;
    public float darkGlobalIntensity;

    [Header("Player PointLight 밝기")]
    public float lightPointIntensity;
    public float darkPointIntensity;

    [Header("Player PointLight 안쪽 반지름 크기")]
    public float lightInnerRadius;
    public float darkInnerRadius;

    [Header("Player PointLight 바깥쪽 반지름 크기")]
    public float lightOuterRadius;
    public float darkOuterRadius;

    [Header("밝기 변화 시간")]
    [SerializeField]
    private float duration = 1f;

    private Sequence seq;

    private void Awake()
    {
        areaStateHolderList = objectParentTrm.GetComponentsInChildren<AreaStateHolder>().ToList();
        arsonAreaStateHolderList = arsonParentTrm.GetComponentsInChildren<AreaStateHolder>().ToList();
        arsonSlotList = arsonParentTrm.GetComponentsInChildren<ArsonSlot>().ToList();
    }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        for (int i = 0; i < lightMapObjs.Length; i++)
        {
            lightMapObjs[i].SetActive(true);
        }

        EventManager.SubEnterRoom(p =>
        {
            player = p;
        });

        EventManager.SubTimeChange(isLight =>
        {
            if (isLight)
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

        EventManager.SubPlayerDead(() =>
        {
            if(seq != null)
            {
                seq.Kill();
            }

            shadowCasterParent.SetActive(false);

            lightMapPoint.gameObject.SetActive(false);
            shadowPoint.gameObject.SetActive(false);

            global.intensity = 1f;

            for (int i = 0; i < lightMapObjs.Length; i++)
            {
                lightMapObjs[i].SetActive(false);
            }

            for (int i = 0; i < areaStateHolderList.Count; i++)
            {
                areaStateHolderList[i].Sr.color = UtilClass.opacityColor;
            }
        });

        EventManager.SubGameOver(goc =>
        {
            Init();
        });

        EventManager.SubExitRoom(() =>
        {
            Init();
        });
    }

    void Update()
    {
        if (player == null || player.isDie) return;

        for (int i = 0; i < areaStateHolderList.Count; i++)
        {
            areaStateHolderList[i].Sr.color = areaStateHolderList[i].AreaState == player.AreaState ? UtilClass.opacityColor : UtilClass.limpidityColor;
        }

        otherList[0].SetActive(player.AreaState == AreaState.BottleStorage);
        otherList[1].SetActive(player.AreaState == AreaState.RefineryInLab);

        if (!ArsonManager.Instance.isArson) return;

        for (int i = 0; i < arsonAreaStateHolderList.Count; i++)
        {
            if (!arsonSlotList[i].isArson) continue;

            arsonSlotList[i].SetActive(arsonAreaStateHolderList[i].AreaState == player.AreaState ? UtilClass.opacityColor : UtilClass.limpidityColor);
        }
    }

    private void Init()
    {
        shadowCasterParent.SetActive(true);

        lightMapPoint.gameObject.SetActive(true);
        shadowPoint.gameObject.SetActive(true);

        global.intensity = lightGlobalIntensity;

        shadowPoint.intensity = lightPointIntensity;

        lightMapPoint.pointLightInnerRadius = lightInnerRadius;
        lightMapPoint.pointLightOuterRadius = lightOuterRadius;

        shadowPoint.pointLightInnerRadius = lightInnerRadius;
        shadowPoint.pointLightOuterRadius = lightOuterRadius;

        for (int i = 0; i < lightMapObjs.Length; i++)
        {
            lightMapObjs[i].SetActive(true);
        }
    }

    public void Dark()
    {
        if (player == null || player.isDie) return;

        if (seq != null)
        {
            seq.Kill();
        }

        seq = DOTween.Sequence();

        seq.Append(DOTween.To(() => global.intensity, x => global.intensity = x, darkGlobalIntensity, duration));

        seq.Join(DOTween.To(() => shadowPoint.intensity, x => shadowPoint.intensity = x, darkPointIntensity, duration));

        seq.Join(DOTween.To(() => shadowPoint.pointLightInnerRadius, x => shadowPoint.pointLightInnerRadius = x, darkInnerRadius, duration));
        seq.Join(DOTween.To(() => shadowPoint.pointLightOuterRadius, x => shadowPoint.pointLightOuterRadius = x, darkOuterRadius, duration));
        seq.Join(DOTween.To(() => lightMapPoint.pointLightInnerRadius, x => lightMapPoint.pointLightInnerRadius = x, darkInnerRadius, duration));
        seq.Join(DOTween.To(() => lightMapPoint.pointLightOuterRadius, x => lightMapPoint.pointLightOuterRadius = x, darkOuterRadius, duration));
    }

    public void Light()
    {
        if (player.isDie) return;

        if (seq != null)
        {
            seq.Kill();
        }

        seq = DOTween.Sequence();

        seq.Append(DOTween.To(() => global.intensity, x => global.intensity = x, lightGlobalIntensity, duration));

        seq.Join(DOTween.To(() => shadowPoint.intensity, x => shadowPoint.intensity = x, lightPointIntensity, duration));

        seq.Join(DOTween.To(() => shadowPoint.pointLightInnerRadius, x => shadowPoint.pointLightInnerRadius = x, lightInnerRadius, duration));
        seq.Join(DOTween.To(() => shadowPoint.pointLightOuterRadius, x => shadowPoint.pointLightOuterRadius = x, lightOuterRadius, duration));
        seq.Join(DOTween.To(() => lightMapPoint.pointLightInnerRadius, x => lightMapPoint.pointLightInnerRadius = x, lightInnerRadius, duration));
        seq.Join(DOTween.To(() => lightMapPoint.pointLightOuterRadius, x => lightMapPoint.pointLightOuterRadius = x, lightOuterRadius, duration));
    }
}
