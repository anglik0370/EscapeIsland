using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetMission : IMission
{
    public MissionType MissionType { get; }
}
