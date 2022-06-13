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

    private Sequence lightSeq;
    private Sequence objSeq;

    private AreaState oldState;

    private void Awake()
    {
        areaStateHolderList = objectParentTrm.GetComponentsInChildren<AreaStateHolder>().ToList();
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
            if(lightSeq != null)
            {
                lightSeq.Kill();
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

        if(oldState != player.AreaState)
        {
            ChangeVisibleObjects(player.AreaState);
        }

        oldState = player.AreaState;
    }

    private void Init()
    {
        if (objSeq != null)
        {
            objSeq.Kill();
        }

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

    public void ChangeVisibleObjects(AreaState areaState)
    {
        if(objSeq != null)
        {
            objSeq.Kill();
        }

        objSeq = DOTween.Sequence();

        otherList[0].SetActive(areaState == AreaState.BottleStorage); //물병 미션
        otherList[1].SetActive(areaState == AreaState.RefineryInLab); //연구소 안 정제소
        otherList[2].SetActive(areaState == AreaState.Refinery); //정제소

        for (int i = 0; i < areaStateHolderList.Count; i++)
        {
            areaStateHolderList[i].Sr.color = UtilClass.limpidityColor;
        }

        var areaObjList = areaStateHolderList.Where(x => x.AreaState == areaState).ToList();

        for (int i = 0; i < areaObjList.Count; i++)
        {
            int j = i;

            if(areaObjList[i].GetComponent<MSObject>() != null)
            {
                //이러면 미션 오브젝트임
                if(areaObjList[i].GetComponent<MSObject>().IsEmpty)
                {
                    //비었으면 만들면 안됨
                    continue;
                }
            }

            objSeq.Join(areaObjList[j].Sr.DOColor(UtilClass.opacityColor, duration));
        }

        if(areaState == AreaState.ShipInside)
        {
            objSeq.Join(otherList[3].GetComponent<SpriteRenderer>().DOColor(UtilClass.limpidityColor, duration)); //바다 끄기
        }

        if(oldState == AreaState.ShipInside)
        {
            objSeq.Join(otherList[3].GetComponent<SpriteRenderer>().DOColor(UtilClass.opacityColor, duration)); //바다 켜기
        }

        if (ArsonManager.Instance.isArson)
        {
            for (int i = 0; i < arsonSlotList.Count; i++)
            {
                arsonSlotList[i].SetActive(UtilClass.limpidityColor);
            }

            var areaArsonList = arsonSlotList.Where(x => x.isArson && x.GetComponent<AreaStateHolder>().AreaState == areaState).ToList();

            for (int i = 0; i < areaArsonList.Count; i++)
            {
                int j = i;
                objSeq.Join(areaArsonList[j].backgroundImg.DOColor(UtilClass.opacityColor, duration));
            }
        }
    }

    public void Dark()
    {
        if (player == null || player.isDie) return;

        if (lightSeq != null)
        {
            lightSeq.Kill();
        }

        lightSeq = DOTween.Sequence();

        lightSeq.Append(DOTween.To(() => global.intensity, x => global.intensity = x, darkGlobalIntensity, duration));

        lightSeq.Join(DOTween.To(() => shadowPoint.intensity, x => shadowPoint.intensity = x, darkPointIntensity, duration));

        lightSeq.Join(DOTween.To(() => shadowPoint.pointLightInnerRadius, x => shadowPoint.pointLightInnerRadius = x, darkInnerRadius, duration));
        lightSeq.Join(DOTween.To(() => shadowPoint.pointLightOuterRadius, x => shadowPoint.pointLightOuterRadius = x, darkOuterRadius, duration));
        lightSeq.Join(DOTween.To(() => lightMapPoint.pointLightInnerRadius, x => lightMapPoint.pointLightInnerRadius = x, darkInnerRadius, duration));
        lightSeq.Join(DOTween.To(() => lightMapPoint.pointLightOuterRadius, x => lightMapPoint.pointLightOuterRadius = x, darkOuterRadius, duration));
    }

    public void Light()
    {
        if (player.isDie) return;

        if (lightSeq != null)
        {
            lightSeq.Kill();
        }

        lightSeq = DOTween.Sequence();

        lightSeq.Append(DOTween.To(() => global.intensity, x => global.intensity = x, lightGlobalIntensity, duration));

        lightSeq.Join(DOTween.To(() => shadowPoint.intensity, x => shadowPoint.intensity = x, lightPointIntensity, duration));

        lightSeq.Join(DOTween.To(() => shadowPoint.pointLightInnerRadius, x => shadowPoint.pointLightInnerRadius = x, lightInnerRadius, duration));
        lightSeq.Join(DOTween.To(() => shadowPoint.pointLightOuterRadius, x => shadowPoint.pointLightOuterRadius = x, lightOuterRadius, duration));
        lightSeq.Join(DOTween.To(() => lightMapPoint.pointLightInnerRadius, x => lightMapPoint.pointLightInnerRadius = x, lightInnerRadius, duration));
        lightSeq.Join(DOTween.To(() => lightMapPoint.pointLightOuterRadius, x => lightMapPoint.pointLightOuterRadius = x, lightOuterRadius, duration));
    }
}
