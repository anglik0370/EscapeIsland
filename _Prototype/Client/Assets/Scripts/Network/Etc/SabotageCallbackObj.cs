using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SabotageCallbackObj : MonoBehaviour
{
    public void TrapCallback()
    {
        Sabotage.Instance.SpawnTrap();
    }
}
