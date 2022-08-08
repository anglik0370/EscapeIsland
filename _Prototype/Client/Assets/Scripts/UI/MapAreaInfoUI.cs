using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapAreaInfoUI : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> profileSpriteList = new List<Sprite>();

    private List<Image> profileImgList = new List<Image>();

    private int userCount;

    private void Awake()
    {
        profileImgList = transform.Find("UserIcons").GetComponentsInChildren<Image>().ToList();

        EventManager.SubEnterRoom(p =>
        {
            Init();
        });

        EventManager.SubGameInit(() =>
        {
            Init();
        });
    }

    private void Init()
    {
        for (int i = 0; i < profileImgList.Count; i++)
        {
            profileImgList[i].color = UtilClass.limpidityColor;
        }
    }

    public void Add(CharacterType type)
    {
        if(userCount < profileImgList.Count)
        {
            profileImgList[++userCount].color = UtilClass.opacityColor;
        }
    }

    public void Sub()
    {
        if(userCount > 0)
        {
            profileImgList[--userCount].color = UtilClass.limpidityColor;
        }
    }
}
