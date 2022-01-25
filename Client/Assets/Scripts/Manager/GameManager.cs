using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<ItemSpawner> spawnerList = new List<ItemSpawner>();
    public List<Refinery> refineryList = new List<Refinery>();

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        spawnerList = GameObject.FindObjectsOfType<ItemSpawner>().ToList();
        refineryList = GameObject.FindObjectsOfType<Refinery>().ToList();
    }
}
