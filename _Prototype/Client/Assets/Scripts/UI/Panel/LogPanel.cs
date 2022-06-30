using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogPanel : MonoBehaviour
{
    private ScrollRect scrollRect;

    [SerializeField]
    private Transform contentTrm;

    [SerializeField]
    private LogText logTextPrefab;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    private void Start()
    {
        CreateLogText("�ȳ��ϼ���");
        CreateLogText("�ȳ��ϼ���ȳ��ϼ���ȳ��ϼ���ȳ��ϼ���");
    }

    public void CreateLogText(string str)
    {
        LogText logText = Instantiate(logTextPrefab, contentTrm);
        logText.SetText(str);
    }
}
