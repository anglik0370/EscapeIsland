using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBattery : MonoBehaviour, IMission
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

    public void Init()
    {
        
    }
}
