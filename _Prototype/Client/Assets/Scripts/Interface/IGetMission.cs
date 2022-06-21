using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetMission : IMission
{
    public bool IsOpen { get; }
    public MissionType MissionType { get; }
}
