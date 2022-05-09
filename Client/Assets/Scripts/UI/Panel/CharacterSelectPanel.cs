using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSelectPanel : Panel
{
    public static CharacterSelectPanel Instance { get; set; }

    private CharacterProfile profilePrefab;
    private List<CharacterSO> charSOList;
    private List<CharacterProfile> profileList = new List<CharacterProfile>();

    [SerializeField]
    private Transform profileParent;

    protected override void Awake()
    {
        Instance = this;

        profilePrefab = Resources.Load<CharacterProfile>("SelectUI/Profile");

        charSOList = Resources.LoadAll<CharacterSO>("CharacterSO/").ToList();

        for (int i = 0; i < charSOList.Count; i++)
        {
            CharacterProfile temp = Instantiate(profilePrefab, profileParent);
            temp.Init(charSOList[i]);
            profileList.Add(temp);
        }

        base.Awake();
    }

    public void SetEnterRoomData(List<int> selectedIdList)
    {
        foreach (CharacterProfile pr in profileList)
        {
            for (int i = 0; i < selectedIdList.Count; i++)
            {
                if(pr.GetSO().id == selectedIdList[i])
                {
                    pr.BtnEnabled(false);
                    selectedIdList.RemoveAt(i);
                    break;
                }
            }

            if (selectedIdList.Count <= 0) break;
        }
    }

    public CharacterProfile GetNotSelectedProfile()
    {
        return profileList.Find(profile => !profile.IsSelected());
    }

    public CharacterProfile GetDefaultProfile()
    {
        return profileList[0];
    }

    public CharacterProfile GetCharacterProfile(int charId)
    {
        return profileList.Find(profile => profile.GetSO().id == charId);
    }
}
