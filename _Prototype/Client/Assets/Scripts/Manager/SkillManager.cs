using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private List<SkillSO> skillList;

    private void Awake()
    {
        skillList = Resources.LoadAll<SkillSO>("SkillSO").ToList();
        skillList.Sort((x, y) => x.id.CompareTo(y.id));

        skillList.ForEach(x => print(x.skillName));
    }
}
