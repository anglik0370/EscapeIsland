using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharInfoPanel : Panel
{
    [Header("ĳ���� ����")]
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

    [Header("��ų ����")]
    [SerializeField]
    private Image skillIconImg;
    [SerializeField]
    private Text skillNameTxt;
    [SerializeField]
    private Text skillCooltimeTxt;
    [SerializeField]
    private Text skillExplanationTxt;

    [Header("��ư��")]
    [SerializeField]
    private Button closeBtn;
    [SerializeField]
    private Button selectBtn;

    [Header("���� ��������Ʈ")]
    [SerializeField]
    private Sprite maleSprite;
    [SerializeField]
    private Sprite femaleSprite;

    [Header("Ÿ�� ��������Ʈ")]
    [SerializeField]
    private Sprite attackSprite;
    [SerializeField]
    private Sprite supportSprite;
    [SerializeField]
    private Sprite utillSprite;

    private CharacterSO curOpenCharSO; // ���� �����ִ� CharacterSO
    public CharacterSO CurOpenCharSO => curOpenCharSO;

    protected override void Start()
    {
        closeBtn.onClick.AddListener(() => Close(false));
        StartCoroutine(CoroutineHandler.Frame(() =>
        {
            SetCharacter sc = NetworkManager.instance.FindSetDataScript<SetCharacter>();
            selectBtn.onClick.AddListener(() => 
            {
                sc.ChangeCharacter(curOpenCharSO);
                Close(false);
            });
        }));
    }

    public void Open(CharacterSO characterSO)
    {
        selectBtn.interactable = true;

        this.curOpenCharSO = characterSO;

        charImg.sprite = characterSO.standImg;
        nameTxt.text = characterSO.charName;
        sexTxt.text = characterSO.sex == Sex.Male ? "����" : "����";
        sexImg.sprite = characterSO.sex == Sex.Male ? maleSprite : femaleSprite;
        jobTxt.text = characterSO.jobName;
        switch (characterSO.skill.skillType)
        {
            case SkillType.Attack:
                skillTypeTxt.text = "������";
                skillTypeImg.sprite = attackSprite;
                break;
            case SkillType.Support:
                skillTypeTxt.text = "������";
                skillTypeImg.sprite = supportSprite;
                break;
            case SkillType.Utill:
                skillTypeTxt.text = "��ƿ��";
                skillTypeImg.sprite = utillSprite;
                break;
        }

        skillIconImg.sprite = characterSO.skill.skillIcon;
        skillNameTxt.text = characterSO.skill.skillName;
        skillCooltimeTxt.text = $"{characterSO.skill.coolTime}��";
        skillExplanationTxt.text = characterSO.skill.skillExplanation;

        base.Open(false);
    }
    
    public void DisableConfirmBtn()
    {
        selectBtn.interactable = false;
    }
}
