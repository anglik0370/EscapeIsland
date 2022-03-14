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
            SetCvsActive(cvsList[JOYSTICK]);
            SetCvsActive(cvsList[INTERACTIONBTN]);
            SetCvsActive(cvsList[SETTINGBTN]);
        });

        EventManager.SubGameStart(p =>
        {
            cvsList.ForEach(x => SetCvsActive(x));
        });

        EventManager.SubGameOver(gameOverCase =>
        {
            cvsList.ForEach(x => SetCvsActive(x, false));
        });

        EventManager.SubExitRoom(() =>
        {
            cvsList.ForEach(x => SetCvsActive(x, false));
        });

        EventManager.SubBackToRoom(() =>
        {
            SetCvsActive(cvsList[JOYSTICK]);
            SetCvsActive(cvsList[INTERACTIONBTN]);
            SetCvsActive(cvsList[SETTINGBTN]);
        });
    }

    private void SetCvsActive(CanvasGroup cvs, bool isEnable = true)
    {
        cvs.alpha = isEnable ? 1f : 0f;
        cvs.interactable = isEnable;
        cvs.blocksRaycasts = isEnable;
    }
}
