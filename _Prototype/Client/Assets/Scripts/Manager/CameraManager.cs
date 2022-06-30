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

        foreach (var texture in texture2DList)
        {
            texture.width = Screen.height;
            texture.height = Screen.width;
        }
    }
}
