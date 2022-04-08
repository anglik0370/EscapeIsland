using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class MissionCoconut : MonoBehaviour
{
    private TouchScreen touchScreen;

    private List<CoconutPalm> coconutPalmList;

    [SerializeField]
    private RectTransform treeTrm;
    private Vector2 originTreePos;
    [SerializeField]
    private List<RectTransform> palmTrmList;
    private List<Vector2> originPalmPosList;

    [Header("Èçµå´Â ¿¬Ãâ °ü·Ã")]
    [SerializeField]
    private float SHAKE_DUTAION = 0.5f;
    [SerializeField]
    private float SHAKE_POWER = 30f;

    private Sequence shakeSeq;

    [Header("¶³¾îÁö´Â ¿¬Ãâ °ü·Ã")]
    [SerializeField]
    private float dropPointY;

    [Header("ÅÍÄ¡ È½¼ö °ü·Ã")]
    [SerializeField]
    private int maxTouch;
    [SerializeField]
    private int touchCount;

    [Header("ÀèÆÌ È®·ü")]
    private float jackPotPercent = 50;

    private void Awake()
    {
        coconutPalmList = new List<CoconutPalm>();
        originPalmPosList = new List<Vector2>();

        touchScreen = GetComponentInChildren<TouchScreen>();
        coconutPalmList = GetComponentsInChildren<CoconutPalm>().ToList();

        for (int i = 0; i < coconutPalmList.Count; i++)
        {
            palmTrmList.Add(coconutPalmList[i].GetComponent<RectTransform>());
        }

        originTreePos = treeTrm.anchoredPosition;

        for (int i = 0; i < palmTrmList.Count; i++)
        {
            originPalmPosList.Add(palmTrmList[i].anchoredPosition);
        }
    }

    private void Start()
    {
        touchScreen.SubTouchEvent(AddTouchCount);
        coconutPalmList.ForEach(x => x.Init());
    }

    public void Init()
    {
        touchScreen.SubTouchEvent(AddTouchCount);
        coconutPalmList.ForEach(x => x.Init());

        for (int i = 0; i < palmTrmList.Count; i++)
        {
            palmTrmList[i].anchoredPosition = originPalmPosList[i];
        }
    }

    private void AddTouchCount()
    {
        CoconutPalm coconutPalm = coconutPalmList.Find(x => !x.IsDropped);

        if (coconutPalm != null)
        {
            if ((touchCount + 1) >= maxTouch)
            {
                if(Random.GetResult(50f))
                {
                    coconutPalmList.ForEach(x => x.Drop(dropPointY));
                }
                else
                {
                    coconutPalm.Drop(dropPointY);
                }

                touchCount = 0;
                return;
            }

            ShakeTree();
            touchCount++;
        }
    }

    private void ShakeTree()
    {
        if(shakeSeq != null)
        {
            shakeSeq.Kill();

            treeTrm.anchoredPosition = originTreePos;

            for (int i = 0; i < palmTrmList.Count; i++)
            {
                if (coconutPalmList[i].IsDropped) continue;

                palmTrmList[i].anchoredPosition = originPalmPosList[i];
            }
        }

        shakeSeq = DOTween.Sequence();

        shakeSeq.Append(treeTrm.DOShakeAnchorPos(SHAKE_DUTAION, SHAKE_POWER));

        for (int i = 0; i < palmTrmList.Count; i++)
        {
            if (coconutPalmList[i].IsDropped) continue;

            int a = i;

            shakeSeq.Join(palmTrmList[a].DOShakeAnchorPos(SHAKE_DUTAION, SHAKE_POWER));
        }
    }
}
