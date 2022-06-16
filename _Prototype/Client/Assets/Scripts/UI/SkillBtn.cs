using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour
{
    private Button btn;
    private Image btnImage;
    private Image coolTimeImg;

    private Player player;
    private SkillSO curSkill => player.curSO.skill;

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
            player = p;
        });
    }

    private void Update()
    {
        if (player == null) return;

        curSkill.UpdateTimer();

        coolTimeImg.fillAmount = curSkill.timer / curSkill.coolTime;
        btnImage.raycastTarget = (curSkill.timer / curSkill.coolTime) <= 0;
    }

    private void UseSkill()
    {
        if (player == null) return;

        curSkill.Callback?.Invoke();
        curSkill.timer = curSkill.coolTime;
    }
}
