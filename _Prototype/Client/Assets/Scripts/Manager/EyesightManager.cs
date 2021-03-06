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
    private List<GameObject> shadowList;

    [SerializeField]
    private GameObject seaObject;

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

    private Area oldArea;

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
            player = p;

            ChangeVisibleObjects(Area.OutSide);
            shadowList.ForEach(x => x.SetActive(false));

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

            ChangeVisibleObjects(Area.OutSide);
            shadowList.ForEach(x => x.SetActive(false));
        });

        EventManager.SubExitRoom(() =>
        {
            Init();
        });
    }

    void Update()
    {
        if (player == null) return;

        if(oldArea != player.Area)
        {
            ChangeVisibleObjects(player.Area);
        }

        oldArea = player.Area;
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

    public void ChangeVisibleObjects(Area area)
    {
        if(objSeq != null)
        {
            objSeq.Kill();
        }

        objSeq = DOTween.Sequence();

        Area flag = Area.OutSide;

        if(Area.OutSide.HasFlag(area))
        {
            flag = Area.OutSide;
        }
        else if(Area.InSide.HasFlag(area))
        {
            flag = Area.InSide;
        }

        Area oldFlag = Area.OutSide;

        if (Area.OutSide.HasFlag(oldArea))
        {
            oldFlag = Area.OutSide;
        }
        else if (Area.InSide.HasFlag(oldArea))
        {
            oldFlag = Area.InSide;
        }

        if (flag == oldFlag) return;

        var areaObjList = areaStateHolderList.Where(x => flag.HasFlag(x.Area)).ToList();
        var otherObjList = areaStateHolderList.Where(x => oldFlag.HasFlag(x.Area)).ToList();

        for (int i = 0; i < otherObjList.Count; i++)
        {
            int j = i;

            objSeq.Join(otherObjList[j].Sr.DOColor(UtilClass.limpidityColor, duration));
        }

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

        shadowList.ForEach(x => x.SetActive(false));

        if(area == Area.EngineRoom)
        {
            shadowList[0].SetActive(true);
        }
        else if(area == Area.ChargeRoom)
        {
            shadowList[1].SetActive(true);
        }
        else if(area == Area.BottleRoom)
        {
            shadowList[2].SetActive(true);
        }
        else if(area == Area.BatteryRoom)
        {
            shadowList[3].SetActive(true);
        }

        if(area == Area.ShipInside)
        {
            objSeq.Join(seaObject.GetComponent<SpriteRenderer>().DOColor(UtilClass.limpidityColor, duration)); //바다 끄기
        }
        else if(oldArea == Area.ShipInside)
        {
            objSeq.Join(seaObject.GetComponent<SpriteRenderer>().DOColor(UtilClass.opacityColor, duration)); //바다 켜기
        }

        if (ArsonManager.Instance.isArson)
        {
            for (int i = 0; i < arsonSlotList.Count; i++)
            {
                arsonSlotList[i].EyeActive(UtilClass.limpidityColor);
            }

            var areaArsonList = arsonSlotList.Where(x => x.isArson && x.GetComponent<AreaStateHolder>().Area == area).ToList();

            for (int i = 0; i < areaArsonList.Count; i++)
            {
                int j = i;
                objSeq.Join(areaArsonList[j].backgroundImg.DOColor(UtilClass.opacityColor, duration));
            }
        }
    }

    public void Dark()
    {
        if (player == null) return;

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
