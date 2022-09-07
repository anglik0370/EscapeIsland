using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour
{
    private RectTransform rect;
    private Button btn;

    private Image btnImage;
    private Image coolTimeImg;

    private Text skillText;

    private Sprite originSprite;

    private SkillSO curSkill => PlayerManager.Instance.Player.curSO.skill;

    [SerializeField]
    private SkillSO amberSkill; //기본이 엠버라 엠버쓰는거

    private bool isGameStart;
    private bool isEnterRoom;

    private bool isPassiveCalled;

    public bool CanTouch => btnImage.raycastTarget;
 
    private bool IsTargetingSkill => curSkill is TargetingSkillSO;
    private bool IsAreaRestrictionSkill => curSkill is AreaRestrictionSkillSO;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        btn = GetComponent<Button>();

        btnImage = GetComponent<Image>();
        coolTimeImg = transform.Find("CooltimeImg").GetComponent<Image>();

        skillText = transform.Find("text").GetComponent<Text>();

        btn.onClick.AddListener(UseSkill);

        originSprite = btnImage.sprite;
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
        if(!isGameStart)
        {
            if(curSkill.skillIcon == null)
            {
                btn.image.sprite = originSprite;
                skillText.text = "Skill";

                rect.sizeDelta = new Vector2(220, 220);
            }
            else
            {
                btn.image.sprite = curSkill.skillIcon;
                skillText.text = "";

                rect.sizeDelta = new Vector2(170, 170);
            }

            return;
        }

        if (IsTargetingSkill)
        {
            TargetingSkillSO so = (TargetingSkillSO)curSkill;
            int targetSocId = PlayerManager.Instance.GetRangeInPlayerId(so.skillRange);

            if (targetSocId == 0)
            {
                coolTimeImg.fillAmount = so.IsCoolTime ? curSkill.timer / curSkill.coolTime : 1f;
                btnImage.raycastTarget = false;

                return;
            }
        }

        if(IsAreaRestrictionSkill)
        {
            AreaRestrictionSkillSO so = (AreaRestrictionSkillSO)curSkill;

            bool isTouching = false;

            foreach (var col in so.colliderList)
            {
                if (Physics2D.IsTouching(col, PlayerManager.Instance.Player.BodyCollider))
                {
                    isTouching = true;
                    break;
                }
            }

            if(!isTouching)
            {
                coolTimeImg.fillAmount = curSkill.IsCoolTime ? curSkill.timer / curSkill.coolTime : 1f;
                btnImage.raycastTarget = false;
                so.isInShip = false;
                return;
            }
        }

        if (!curSkill.isPassive && !PlayerManager.Instance.Player.IsSturned)
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

        if(IsAreaRestrictionSkill)
        {
            ((AreaRestrictionSkillSO)curSkill).isInShip = true;
        }

        curSkill.Callback?.Invoke();
        curSkill.timer = curSkill.coolTime;

    }
}
