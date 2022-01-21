using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("다수의 PopupManager가 실행중");
            return;
        }
        instance = this;

    }
}
