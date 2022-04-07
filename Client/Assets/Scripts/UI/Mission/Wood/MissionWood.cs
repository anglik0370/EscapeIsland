using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionWood : MonoBehaviour
{
    [SerializeField]
    private List<WoodBranch> brnachList;

    private void Awake()
    {
        brnachList = GetComponentsInChildren<WoodBranch>().ToList();
    }
}
