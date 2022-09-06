using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProfile : MonoBehaviour
{
    [SerializeField]
    private CharacterSO charSO;

    [SerializeField]
    private Button selectBtn;
    [SerializeField]
    private Image profileImg;
    [SerializeField]
    private Text nameTxt;
    [SerializeField]
    private Image sexImg;

    [SerializeField]
    private CanvasGroup maskImg;

    [SerializeField]
    private Sprite maleSprite;
    [SerializeField]
    private Sprite femaleSprite;

    private void Start()
    {
        selectBtn.onClick.AddListener(() => CharacterSelectPanel.Instance.CharInfoPanel.Open(charSO));
    }

    public void Init(CharacterSO so)
    {
        charSO = so;

        profileImg.sprite = so.profileImg;
        nameTxt.text = so.charName;

        switch (so.sex)
        {
            case Sex.Male:
                sexImg.sprite = maleSprite;
                break;
            case Sex.Female:
                sexImg.sprite = femaleSprite;
                break;
        }
    }
    public CharacterSO GetSO()
    {
        return charSO;
    }

    public bool IsSelected()
    {
        return !selectBtn.enabled;
    }

    public void SelectBtn(bool enabled)
    {
        selectBtn.enabled = enabled;
    }

    public void MaskOnOff(bool on)
    {
        maskImg.alpha = on ? 1f : 0f;
    }

    public void BtnEnabled(bool enable)
    {
        if(!enable && charSO.id <= 0)
        {
            return;
        }

        SelectBtn(enable);
        MaskOnOff(!enable);

        if(CharacterSelectPanel.Instance.CharInfoPanel.CurOpenCharSO == charSO)
        {
            CharacterSelectPanel.Instance.CharInfoPanel.DisableConfirmBtn();
        }
    }
}
