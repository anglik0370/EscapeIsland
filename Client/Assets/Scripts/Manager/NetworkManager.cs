using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("다수의 NetworkManager가 실행중");
            return;
        }
        instance = this;
    }
}
