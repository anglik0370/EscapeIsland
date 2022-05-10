using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionHandler : MonoBehaviour,IMsgHandler
{
    public void HandleMsg(string payload)
    {
        ItemSpawnerVO vo = JsonUtility.FromJson<ItemSpawnerVO>(payload);
        ItemAndStorage.SetMissionCool(vo);
    }
}
