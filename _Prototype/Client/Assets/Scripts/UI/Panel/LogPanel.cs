using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogPanel : MonoBehaviour
{
    public static LogPanel Instance { get; private set; }

    private ScrollRect scrollRect;

    [SerializeField]
    private Transform contentTrm;

    [SerializeField]
    private LogText logTextPrefab;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        scrollRect = GetComponent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 0f;
    }
    public void CreateLogText(string str)
    {
        LogText logText = Instantiate(logTextPrefab, contentTrm);
        logText.SetText(str);
    }
}
