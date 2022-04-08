using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionWood : MonoBehaviour
{
    [SerializeField]
    private List<BranchMObj> brnachList;

    private void Awake()
    {
        brnachList = GetComponentsInChildren<BranchMObj>().ToList();
    }
}
