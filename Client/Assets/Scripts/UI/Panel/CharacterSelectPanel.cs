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
    private Dictionary<int,List<GameObject>> charPool = new Dictionary<int,List<GameObject>>();

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
            charPool.Add(charSOList[i].id, new List<GameObject>());
            profileList.Add(temp);
        }

        EventManager.SubExitRoom(InitEnable);
        EventManager.SubBackToRoom(InitEnable);

        base.Awake();
    }

    public GameObject GetCharacterObj(int id)
    {
        GameObject obj = null;

        List<GameObject> objList = charPool[id];

        for (int i = 0; i < objList.Count; i++)
        {
            if(!objList[i].activeSelf)
            {
                obj = objList[i];
                break;
            }
        }
        if(obj == null)
        {
            obj = Instantiate(GetCharacterProfile(id).GetSO().playerPrefab);
        }

        return obj;
    }


    public void InitEnable()
    {
        foreach (CharacterProfile profile in profileList)
        {
            profile.BtnEnabled(true);
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
