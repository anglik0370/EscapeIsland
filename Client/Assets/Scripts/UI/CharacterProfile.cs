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
    private Color maleColor;
    [SerializeField]
    private Color femaleColor;

    private void Start()
    {
        selectBtn.onClick.AddListener(() => NetworkManager.instance.SetCharacter(charSO));
    }

    public void Init(CharacterSO so)
    {
        charSO = so;

        profileImg.sprite = so.profileImg;
        nameTxt.text = so.charName;

        switch (so.sex)
        {
            case Sex.Male:
                sexImg.color = maleColor;
                break;
            case Sex.Female:
                sexImg.color = femaleColor;
                break;
        }
    }
    public CharacterSO GetSO()
    {
        return charSO != null ? charSO : null;
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
        SelectBtn(enable);
        MaskOnOff(!enable);
    }
}
