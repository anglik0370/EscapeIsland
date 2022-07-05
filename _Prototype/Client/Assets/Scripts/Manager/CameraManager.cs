using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private List<RenderTexture> texture2DList;

    private void Start()
    {
        if (texture2DList.Count <= 0) return;

#if UNITY_EDITOR
        foreach (var texture in texture2DList)
        {
            texture.width = Screen.width;
            texture.height = Screen.height;
        }
#elif UNITY_ANDROID
        foreach (var texture in texture2DList)
        {
            texture.width = Screen.height;
            texture.height = Screen.width;
        }
#else
        foreach (var texture in texture2DList)
        {
            texture.width = Screen.width;
            texture.height = Screen.height;
        }
#endif
    }
}
