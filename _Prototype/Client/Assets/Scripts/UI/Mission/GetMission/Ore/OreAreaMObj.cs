using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OreAreaMObj : MonoBehaviour
{
    private Image img;
    private TouchScreen touchScreen;
    private MissionOre missionOre;

    [SerializeField]
    private List<Sprite> spriteList;

    [SerializeField]
    private int touchCnt = 0;

    [SerializeField]
    private AudioClip touchClip;

    private void Awake()
    {
        img = GetComponent<Image>();
        touchScreen = GetComponent<TouchScreen>();
        missionOre = GetComponentInParent<MissionOre>();
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
        if (missionOre.CurOreArea == null)
        {
            missionOre.SetCurOreArea(this);
        }

        if(missionOre.CurOreArea != this)
        {
            return;
        }

        SoundManager.Instance.PlaySFX(touchClip);
        img.sprite = spriteList[++touchCnt];

        if (touchCnt >= spriteList.Count - 2) //-2�� ������ ���� �ϳ��� ���ߵǰ� ó���͵� �� �־��
        {
            //������� �Դٴ°� 6���� �� �����ٴ� �Ҹ���
            img.raycastTarget = false;
            missionOre.SetCurOreArea(null);
            return;
        }
    }
}
