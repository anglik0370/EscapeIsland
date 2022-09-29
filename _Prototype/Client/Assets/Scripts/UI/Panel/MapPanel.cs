using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MapPanel : Panel
{
    public static MapPanel Instance { get; private set; }

    [SerializeField]
    private SerializableDictionary<Area, MapAreaInfoUI> mapAreaInfoDic = new SerializableDictionary<Area, MapAreaInfoUI>();

    protected override void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        base.Awake();

        MapAreaInfoUI[] uis = GetComponentsInChildren<MapAreaInfoUI>();

        for (int i = 0; i < uis.Length; i++)
        {
            mapAreaInfoDic.Add(uis[i].Area, uis[i]);
        }
    }

    public override void Open(bool isTweenSkip = false)
    {
        if(isTweenSkip)
        {
            cvs.alpha = 1f;
            cvs.blocksRaycasts = true;
            cvs.interactable = true;
        }
        else
        {
            if (seq != null)
            {
                seq.Kill();
            }

            seq = DOTween.Sequence();

            seq.Append(cvs.DOFade(1f, TWEEN_DURATION));
            seq.AppendCallback(() =>
            {
                cvs.blocksRaycasts = true;
                cvs.interactable = true;
            });
        }
        SoundManager.Instance.PlayBtnSfx();
    }

    public override void Close(bool isTweenSkip = false)
    {
        base.Close(isTweenSkip);
        SoundManager.Instance.PlayBtnSfx();
    }

    public void InitMapAreaInfoUI()
    {
        foreach (var item in mapAreaInfoDic)
        {
            item.Value.Init();
        }
    }

    public MapAreaInfoUI GetMapAreaInfoUI(Area area)
    {
        MapAreaInfoUI mapAreaInfoUI = null;

        if(mapAreaInfoDic.ContainsKey(area))
        { 
            mapAreaInfoUI = mapAreaInfoDic[area];
        }

        if(mapAreaInfoUI == null)
        {
            mapAreaInfoUI = mapAreaInfoDic[Area.Altar];
        }

        return mapAreaInfoUI;
    }
}
