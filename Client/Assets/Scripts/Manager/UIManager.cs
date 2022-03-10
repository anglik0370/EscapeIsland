using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public CanvasGroup panels;

    public Text warningText;

    private WaitForSeconds waitOneSeconds;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        SetPanelActive(false);

        waitOneSeconds = new WaitForSeconds(1f);
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

    IEnumerator WarningLog(string msg)
    {
        warningText.text = msg;
        warningText.enabled = true;

        yield return waitOneSeconds;

        warningText.enabled = false;
    }

    public void SetWarningText(string msg)
    {
        StartCoroutine(WarningLog(msg));
    }
}
