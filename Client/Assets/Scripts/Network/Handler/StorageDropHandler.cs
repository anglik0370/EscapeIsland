using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageDropHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        print("StorageDrop");
        NetworkManager.instance.SetItemStorage(int.Parse(payload));
    }
}
