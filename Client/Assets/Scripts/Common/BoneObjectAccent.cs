using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoneObjectAccent : MonoBehaviour
{
    [SerializeField]
    List<SpriteRenderer> srList;

    public void Awake()
    {
        srList = GetComponentsInChildren<SpriteRenderer>().ToList();

        foreach (var sr in srList)
        {
            sr.color = UtilClass.limpidityColor;
        }
    }

    public void Enable(List<SpriteRenderer> spriteList, bool isFlip)
    {
        for (int i = 0; i < spriteList.Count; i++)
        {
            srList[i].sprite = spriteList[i].sprite;

            srList[i].gameObject.transform.position = spriteList[i].gameObject.transform.position;
            srList[i].gameObject.transform.localScale = spriteList[i].gameObject.transform.localScale;
            srList[i].gameObject.transform.rotation = spriteList[i].gameObject.transform.rotation;

            srList[i].color = UtilClass.opacityColor;
        }
    }

    public void Disable()
    {
        foreach (var sr in srList)
        {
            sr.color = UtilClass.limpidityColor;
        }
    }
}
