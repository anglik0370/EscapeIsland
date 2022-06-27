using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SabotageCallbackObj : MonoBehaviour
{
    public void TrapCallback()
    {
        Sabotage.Instance.SpawnTrap();
    }

    public void ArsonCallback()
    {
        ArsonManager.Instance.StartArson();
    }

    public void DoorCallback()
    {
        Sabotage.Instance.CloseDoor();
    }
}
