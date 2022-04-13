using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OreAreaMObj : MonoBehaviour
{
    private Image img;
    private TouchScreen touchScreen;

    [SerializeField]
    private List<Sprite> spriteList;

    [SerializeField]
    private int touchCnt = 0;

    private void Awake()
    {
        img = GetComponent<Image>();
        touchScreen = GetComponent<TouchScreen>();
    }

    private void Start()
    {
        touchScreen.SubTouchEvent(OnTouch);

        Init();
    }

    public void Init()
    {
        touchCnt = 0;

        img.sprite = spriteList[0];

        img.raycastTarget = true;
    }

    private void OnTouch()
    {
        if(touchCnt >= spriteList.Count - 2) //-2인 이유는 원래 하나는 빼야되고 처음것도 들어가 있어서임
        {
            //여기까지 왔다는건 6번을 다 눌렀다는 소리임
            img.raycastTarget = false;
        }

        img.sprite = spriteList[++touchCnt];
    }
}
