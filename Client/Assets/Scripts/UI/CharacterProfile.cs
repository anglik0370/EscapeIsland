using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProfile : MonoBehaviour
{
    [SerializeField]
    private CharacterSO charSO;

    [SerializeField]
    private Image profileImg;
    [SerializeField]
    private Text nameTxt;
    [SerializeField]
    private Image sexImg;

    [SerializeField]
    private Color maleColor;
    [SerializeField]
    private Color femaleColor;

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
}
