using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour
{
    private Button btn;
    private Image btnImage;
    private Image coolTimeImg;

    private SkillSO curSkill => PlayerManager.Instance.Player.curSO.skill;

    private bool isGameStart;
    private bool isEnterRoom;

    private bool isPassiveCalled;

    public bool CanTouch => btnImage.raycastTarget;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btnImage = GetComponent<Image>();
        coolTimeImg = transform.Find("CooltimeImg").GetComponent<Image>();

        btn.onClick.AddListener(UseSkill);
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            isEnterRoom = true;
            isPassiveCalled = false;
        });

        EventManager.SubGameStart(p =>
        {
            isGameStart = true;
        });

        EventManager.SubExitRoom(() =>
        {
            isGameStart = false;
            isEnterRoom = false;
            isPassiveCalled = false;
        });

        EventManager.SubBackToRoom(() =>
        {
            isGameStart = false;
            isPassiveCalled = false;
        });
    }

    private void Update()
    {
        if (!isEnterRoom) return;

        if(isGameStart)
        {
            if(curSkill.isPassive)
            {
                if (!isPassiveCalled)
                {
                    curSkill.Callback?.Invoke();
                    isPassiveCalled = true;
                }
            }
            else
            {
                curSkill.UpdateTimer();
            }
        }

        UpdateImage();
    }

    private void UpdateImage()
    {
        if(isGameStart && !curSkill.isPassive && !PlayerManager.Instance.Player.IsSturned)
        {
            coolTimeImg.fillAmount = curSkill.timer / curSkill.coolTime;
            btnImage.raycastTarget = (curSkill.timer / curSkill.coolTime) <= 0;
        }
        else
        {
            coolTimeImg.fillAmount = 1f;
            btnImage.raycastTarget = false;
        }
    }

    private void UseSkill()
    {
        if (!isEnterRoom || !isGameStart || curSkill.isPassive || PlayerManager.Instance.Player.IsSturned) return;

        curSkill.Callback?.Invoke();
        curSkill.timer = curSkill.coolTime;
    }
}
