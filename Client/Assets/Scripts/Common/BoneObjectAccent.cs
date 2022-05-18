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

    public void Enable(CharacterSO charSO, bool isFlip)
    {
        
    }

    public void Disable()
    {
        foreach (var sr in srList)
        {
            sr.color = UtilClass.limpidityColor;
        }
    }
}
