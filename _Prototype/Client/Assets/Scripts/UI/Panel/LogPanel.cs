using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogPanel : MonoBehaviour
{
    private ScrollRect scrollRect;

    

    private const int MAX_CHAR_CNT = 16; //한줄에 최대 몇글자인지

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
