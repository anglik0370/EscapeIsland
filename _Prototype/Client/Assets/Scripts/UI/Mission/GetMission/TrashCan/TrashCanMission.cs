using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanMission : MonoBehaviour, IGetMission
{
    private CanvasGroup cvs;
    public CanvasGroup Cvs => cvs;

    [SerializeField]
    private MissionType missionType;
    public MissionType MissionType => missionType;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
    }

    public void Close()
    {
        
    }

    public void Open()
    {
        
    }
}
