using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<ItemConverter> refineryList = new List<ItemConverter>();

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        refineryList = GameObject.FindObjectsOfType<ItemConverter>().ToList();
    }
}
