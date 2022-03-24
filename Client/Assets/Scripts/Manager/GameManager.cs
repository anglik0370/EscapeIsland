using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<InteractionHandlerSO> interactionHandlerSOList = new List<InteractionHandlerSO>();

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        foreach(var handler in interactionHandlerSOList)
        {
            switch (handler.interactoinCase)     
            {
                case InteractionCase.Nothing:
                    break;
                case InteractionCase.KillPlayer:
                    break;
                case InteractionCase.OpenConverter:
                    break;
                case InteractionCase.OpenStorage:
                    break;
                case InteractionCase.EmergencyMeeting:
                    break;
                case InteractionCase.ReportDeadbody:
                    break;
                case InteractionCase.PickUpItem:
                    break;
                case InteractionCase.GameStart:
                    break;
                case InteractionCase.SelectCharacter:
                    break;
                default:
                    break;
            }
        }
    }
}
