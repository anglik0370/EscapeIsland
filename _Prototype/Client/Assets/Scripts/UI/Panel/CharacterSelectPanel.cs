using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPanel : Panel
{
    public static CharacterSelectPanel Instance { get; set; }

    private CharInfoPanel infoPanel;

    [SerializeField]
    private CharacterProfile profilePrefab;
    private List<CharacterSO> charSOList;
    private List<CharacterProfile> profileList = new List<CharacterProfile>();
    private Dictionary<int,List<GameObject>> charPool = new Dictionary<int,List<GameObject>>();

    [SerializeField]
    private SerializableDictionary<int, bool> profileEnableDic = new SerializableDictionary<int, bool>();

    [SerializeField]
    private Transform profileParent;

    [SerializeField]
    private ScrollRect charRect;

    [SerializeField]
    private CharInfoPanel characterInfoPanel;
    public CharInfoPanel CharInfoPanel => characterInfoPanel;

    protected override void Awake()
    {
        Instance = this;

        infoPanel = GetComponentInChildren<CharInfoPanel>();

        charSOList = Resources.LoadAll<CharacterSO>("CharacterSO/").ToList();

        for (int i = 0; i < charSOList.Count; i++)
        {
            CharacterProfile temp = Instantiate(profilePrefab, profileParent);
            temp.Init(charSOList[i]);
            charPool.Add(charSOList[i].id, new List<GameObject>());
            profileEnableDic.Add(charSOList[i].id, false);
            profileList.Add(temp);
        }

        charRect.verticalNormalizedPosition = 1f;
        EventManager.SubExitRoom(InitEnable);

        base.Awake();
    }

    protected override void Start()
    {
        EventManager.SubEnterRoom(p => 
        {
            Close(true);
            infoPanel.Close(true);
        });

        EventManager.SubGameStart(p =>
        {
            Close(true);
        });
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
            charPool[id].Add(obj);
        }

        return obj;
    }

    public override void Close(bool isTweenSkip = false)
    {
        base.Close(isTweenSkip); SoundManager.Instance.PlayBtnSfx();
    }

    public void InitEnable()
    {
        foreach (CharacterProfile profile in profileList)
        {
            profileEnableDic[profile.GetSO().id] = false;
        }
    }

    public void SetCharecterSelection(int charId, bool enable)
    {
        profileEnableDic[charId] = enable;

        if(CharInfoPanel.CurOpenCharSO != null && CharInfoPanel.CurOpenCharSO.id == charId)
        {
            CharInfoPanel.SetConfimBtnEnable(!enable);
        }
    }

    public bool GetCharacterSelection(int charId)
    {
        return profileEnableDic[charId];
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
