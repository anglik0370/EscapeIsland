using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogText : MonoBehaviour
{
    private Text text;
    private RectTransform rect;

    private const int CHAR_MAX_CNT = 16; //���ٿ� �ִ� ���������
    private const int HEIGHT = 45; //�ؽ�Ʈ�� ����

    private void Awake()
    {
        text = GetComponent<Text>();
        rect = GetComponent<RectTransform>();
    }

    public void SetText(string str)
    {
        int lineBreakCnt = -1;
        int tempLength = str.Length;

        while (tempLength > 0)
        {
            lineBreakCnt++;
            tempLength -= CHAR_MAX_CNT;
        }

        text.text = str;

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, (lineBreakCnt + 1) * HEIGHT);
    }
}
