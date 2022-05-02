using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMission
{
    public CanvasGroup Cvs { get; }
    public MissionType MissionType { get; }

    public void Open();
    public void Close();
}
