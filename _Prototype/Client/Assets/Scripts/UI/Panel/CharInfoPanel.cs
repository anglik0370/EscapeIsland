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

    [Header("타입 스프라이트")]
    [SerializeField]
    private Sprite attackSprite;
    [SerializeField]
    private Sprite supportSprite;
    [SerializeField]
    private Sprite utillSprite;

    private CharacterSO curOpenCharSO; // 현재 열려있는 CharacterSO
    public CharacterSO CurOpenCharSO => curOpenCharSO;

    protected override void Start()
    {
        closeBtn.onClick.AddListener(() =>
        {
            Close(false);
            SoundManager.Instance.PlayBtnSfx();
        });

        StartCoroutine(CoroutineHandler.Frame(() =>
        {
            SetCharacter sc = NetworkManager.instance.FindSetDataScript<SetCharacter>();
            selectBtn.onClick.AddListener(() =>
            {
                sc.ChangeCharacter(curOpenCharSO);
                CharacterSelectPanel.Instance.Close(true);
                SoundManager.Instance.PlayBtnSfx();
                Close(false);
            });
        }));
    }

    public void Open(CharacterSO characterSO)
    {
        this.curOpenCharSO = characterSO;

        selectBtn.interactable = !CharacterSelectPanel.Instance.GetCharacterSelection(characterSO.id);

        charImg.sprite = characterSO.standImg;
        nameTxt.text = $"{characterSO.charName}";
        sexTxt.text = characterSO.sex == Sex.Male ? "남자" : "여자";
        sexImg.sprite = characterSO.sex == Sex.Male ? maleSprite : femaleSprite;
        jobTxt.text = $"{characterSO.jobName}";
        switch (characterSO.skill.skillType)
        {
            case SkillType.Attack:
                skillTypeTxt.text = "공격형";
                skillTypeImg.sprite = attackSprite;
                break;
            case SkillType.Support:
                skillTypeTxt.text = "지원형";
                skillTypeImg.sprite = supportSprite;
                break;
            case SkillType.Utill:
                skillTypeTxt.text = "유틸형";
                skillTypeImg.sprite = utillSprite;
                break;
        }

        skillIconImg.sprite = characterSO.skill.skillIcon;
        skillNameTxt.text = characterSO.skill.skillName;
        skillCooltimeTxt.text = $"쿨타임 {characterSO.skill.coolTime}초";
        skillExplanationTxt.text = characterSO.skill.skillExplanation;

        base.Open(false);
    }

    public void SetConfimBtnEnable(bool enable)
    {
        selectBtn.interactable = enable;
    }
}
