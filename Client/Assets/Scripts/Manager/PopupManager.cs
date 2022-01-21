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
            Debug.LogError("�ټ��� PopupManager�� ������");
            return;
        }
        instance = this;

    }
}
