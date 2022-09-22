using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogText : MonoBehaviour
{
    private Text text;
    private RectTransform rect;

    private const int CHAR_MAX_CNT = 20; //���ٿ� �ִ� ���������
    private const int HEIGHT = 45; //�ؽ�Ʈ�� ����

    private void Awake()
    {
        text = GetComponent<Text>();
        rect = GetComponent<RectTransform>();
    }

    public void SetText(string str)
    {
        string tmpStr = str;

        int loopCnt = 0;

        while (tmpStr.Contains("<"))
        {
            int startIdx = tmpStr.IndexOf("<") - 1;
            int endIdx = tmpStr.IndexOf(">") + 1;

            string frontStr = tmpStr.Substring(0, startIdx);
            string backStr = tmpStr.Substring(endIdx, tmpStr.Length - endIdx);

            tmpStr = frontStr + backStr;

            if(loopCnt > 10000)
            {
                print("�ѹ�����");
                break;
            }    
            else
            {
                loopCnt++;
            }
        }

        int lineBreakCnt = -1;
        int tempLength = tmpStr.Length;
        
        while (tempLength > 0)
        {
            lineBreakCnt++;
            tempLength -= CHAR_MAX_CNT;
        }

        text.text = str;

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, (lineBreakCnt + 1) * HEIGHT);
    }
}
