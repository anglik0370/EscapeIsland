using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSelectPanel : Panel
{
    private CharacterProfile profilePrefab;
    private List<CharacterSO> charSOList;

    [SerializeField]
    private Transform profileParent;

    protected override void Awake()
    {
        profilePrefab = Resources.Load<CharacterProfile>("SelectUI/Profile");

        charSOList = Resources.LoadAll<CharacterSO>("CharacterSO/").ToList();

        for (int i = 0; i < charSOList.Count; i++)
        {
            CharacterProfile temp = Instantiate(profilePrefab, profileParent);
            temp.Init(charSOList[i]);
        }

        base.Awake();
    }

    private void Start()
    {
        EventManager.SubGameOver(gos => Close(true));
    }
}
