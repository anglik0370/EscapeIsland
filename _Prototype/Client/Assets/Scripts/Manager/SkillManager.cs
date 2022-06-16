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
        skillList.Sort((x, y) => x.id.CompareTo(y.id)); //아이디 순으로 정렬

        skillList[0].Callback.AddListener(() => print("앰버 스킬 사용"));
    }
}
