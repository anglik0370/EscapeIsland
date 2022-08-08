using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapAreaInfoUI : MonoBehaviour
{
    [SerializeField]
    private Area area;
    public Area Area => area;

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

    public void Init()
    {
        userCount = 0;

        for (int i = 0; i < profileImgList.Count; i++)
        {
            profileImgList[i].color = UtilClass.limpidityColor;
        }
    }

    public void Add(Sprite sprite)
    {
        if(userCount < profileImgList.Count)
        {
            userCount++;

            profileImgList[userCount].sprite = sprite;
            profileImgList[userCount].color = UtilClass.opacityColor;
        }
    }
}
