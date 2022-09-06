using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharInfoPanel : Panel
{
    [Header("캐릭터 정보")]
    [SerializeField]
    private Image charImg;
    [SerializeField]
    private Text nameTxt;
    [SerializeField]
    private Text sexTxt;
    [SerializeField]
    private Image sexImg;
    [SerializeField]
    private Text jobTxt;
    [SerializeField]
    private Text skillTypeTxt;
    [SerializeField]
    private Image skillTypeImg;

    [Header("스킬 정보")]
    [SerializeField]
    private Image skillIconImg;
    [SerializeField]
    private Text skillNameTxt;
    [SerializeField]
    private Text skillCooltimeTxt;
    [SerializeField]
    private Text skillExplanationTxt;

    [Header("버튼들")]
    [SerializeField]
    private Button closeBtn;
    [SerializeField]
    private Button selectBtn;

    [Header("성별 스프라이트")]
    [SerializeField]
    private Sprite maleSprite;
    [SerializeField]
    private Sprite femaleSprite;

    private CharacterSO curOpenCharSO; // 현재 열려있는 CharacterSO

    protected override void Start()
    {
        closeBtn.onClick.AddListener(() => Close(false));
        StartCoroutine(CoroutineHandler.Frame(() =>
        {
            SetCharacter sc = NetworkManager.instance.FindSetDataScript<SetCharacter>();
            selectBtn.onClick.AddListener(() => sc.ChangeCharacter(curOpenCharSO));
        }));
    }

    public void Open(CharacterSO characterSO)
    {
        this.curOpenCharSO = characterSO;

        charImg.sprite = characterSO.standImg;
        nameTxt.text = characterSO.charName;
        sexTxt.text = characterSO.sex == Sex.Male ? "남자" : "여자";
        sexImg.sprite = characterSO.sex == Sex.Male ? maleSprite : femaleSprite;
        jobTxt.text = characterSO.jobName;
        switch (characterSO.skill.skillType)
        {
            case SkillType.Attack:
                skillTypeTxt.text = "공격형";
                break;
            case SkillType.Support:
                skillTypeTxt.text = "지원형";
                break;
            case SkillType.Utill:
                skillTypeTxt.text = "유틸형";
                break;
        }

        skillIconImg.sprite = characterSO.skill.skillIcon;
        skillNameTxt.text = characterSO.skill.skillName;
        skillCooltimeTxt.text = $"{characterSO.skill.coolTime}초";
        skillExplanationTxt.text = characterSO.skill.skillExplanation;

        base.Open(false);
    }
}
