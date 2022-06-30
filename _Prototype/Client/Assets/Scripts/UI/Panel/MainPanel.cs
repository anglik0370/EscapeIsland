using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : MonoBehaviour
{
    public static MainPanel Instance { get; private set; }

    public const int JOYSTICK = 0;
    public const int INTERACTIONBTN = 1;
    public const int INVENTORY = 2;
    public const int DAYTEXT = 3;
    public const int PROGRESS = 4;
    public const int SETTINGBTN = 5;
    public const int MAPBTN = 6;
    public const int INGAMETIMETEXT = 7;
    public const int USERCOUNT = 8;
    public const int CHATBTN = 9;

    [SerializeField]
    private List<CanvasGroup> cvsList = new List<CanvasGroup>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        cvsList.ForEach(x => SetCvsActive(x, false));
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            cvsList.ForEach(x => SetCvsActive(x, false));

            SetCvsActive(cvsList[JOYSTICK]);
            SetCvsActive(cvsList[INTERACTIONBTN]);
            SetCvsActive(cvsList[SETTINGBTN]);
            SetCvsActive(cvsList[USERCOUNT]);
            SetCvsActive(cvsList[CHATBTN]);
        });

        EventManager.SubGameStart(p =>
        {
            cvsList.ForEach(x => SetCvsActive(x));
            SetCvsActive(cvsList[USERCOUNT], false);
        });

        EventManager.SubExitRoom(() =>
        {
            cvsList.ForEach(x => SetCvsActive(x, false));
        });

        EventManager.SubBackToRoom(() =>
        {
            cvsList.ForEach(x => SetCvsActive(x, false));

            SetCvsActive(cvsList[JOYSTICK]);
            SetCvsActive(cvsList[INTERACTIONBTN]);
            SetCvsActive(cvsList[SETTINGBTN]);
            SetCvsActive(cvsList[USERCOUNT]);
        });
    }

    private void SetCvsActive(CanvasGroup cvs, bool isEnable = true)
    {
        cvs.alpha = isEnable ? 1f : 0f;
        cvs.interactable = isEnable;
        cvs.blocksRaycasts = isEnable;
    }
}
