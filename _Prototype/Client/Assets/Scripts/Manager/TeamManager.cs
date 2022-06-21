using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TeamManager : MonoBehaviour
{
    //Enum�� �ִ� �� ������� �־���ߵ�
    [SerializeField]
    private List<Transform> shipParentTrmList = new List<Transform>();

    private void Awake()
    {
        for (int i = 0; i < shipParentTrmList.Count; i++)
        {
            shipParentTrmList[i].GetComponentsInChildren<ItemStorage>().ToList().ForEach(x => x.SetTeam((Team)(i + 1)));
        }
        
        for (int i = 0; i < shipParentTrmList.Count; i++)
        {
            shipParentTrmList[i].GetComponentsInChildren<MSObject>().ToList().ForEach(x => x.SetTeam((Team)(i + 1)));
        }
    }
}
