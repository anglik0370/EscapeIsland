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
        if(touchCnt >= spriteList.Count - 2) //-2�� ������ ���� �ϳ��� ���ߵǰ� ó���͵� �� �־��
        {
            //������� �Դٴ°� 6���� �� �����ٴ� �Ҹ���
            img.raycastTarget = false;
        }

        img.sprite = spriteList[++touchCnt];
    }
}
