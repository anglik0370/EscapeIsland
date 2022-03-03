using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public CanvasGroup panels;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        SetPanelActive(false);
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            SetPanelActive(true);
        });

        EventManager.SubExitRoom(() =>
        {
            SetPanelActive(false);
        });
    }

    public static void SetPanelActive(bool isEnable)
    {
        Instance.panels.alpha = isEnable ? 1f : 0f;
        Instance.panels.interactable = isEnable;
        Instance.panels.blocksRaycasts = isEnable;
    }
}
