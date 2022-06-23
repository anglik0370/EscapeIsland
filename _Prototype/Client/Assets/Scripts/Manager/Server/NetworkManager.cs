using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Linq;
using System;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public GameObject playerPrefab;

    public int socketId = -1;
    public string socketName;
    public int roomNum;

    public object lockObj = new object();

    private Dictionary<int, Player> playerList = new Dictionary<int, Player>();
    private List<ISetAble> setDataScriptList = new List<ISetAble>();
    private Queue<int> removeSocketQueue = new Queue<int>();

    public Transform setDataScriptsParent;

    private bool isLogin = false;
    private bool isChangeTeam = false;
    private bool isTimeRefresh = false;

    private ChangeTeamVO changeTeamVO;
    private TimeVO timeData;

    private Player user = null;
    public Player User
    {
        get { return user; }
        set { user = value; }
    }

    public GameObject map;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("�ټ��� NetworkManager�� ������");
            return;
        }
        instance = this;

    }

    private void Start()
    {
        setDataScriptList = setDataScriptsParent.GetComponents<ISetAble>().ToList();
        PoolManager.CreatePool<Player>(playerPrefab, transform, 30);

        EventManager.SubEnterRoom(p => user = p);

        StartCoroutine(CoroutineHandler.Frame(() =>
        {
            map.SetActive(false);
        }));
    }

    public T FindSetDataScript<T>() where T : ISetAble
    {
        return (T)setDataScriptList.Find(x => x.GetType() == typeof(T));
    }
    
    public static void SetLoginData(string name, int socketId)
    {
        lock(instance.lockObj)
        {
            instance.socketName = name;
            instance.socketId = socketId;
            instance.isLogin = true;
        }
    }

    public static void DisconnectUser(int id)
    {
        lock(instance.lockObj)
        {
            instance.removeSocketQueue.Enqueue(id);
        }
    }

    public static void SetChangeTeam(ChangeTeamVO changeTeamVO)
    {
        lock(instance.lockObj)
        {
            instance.changeTeamVO = changeTeamVO;
            instance.isChangeTeam = true;
        }
    }

    public static void SetTimeData(TimeVO vo)
    {
        lock(instance.lockObj)
        {
            instance.timeData = vo;
            instance.isTimeRefresh = true;
        }
    }

    private void Update()
    {
        if(isLogin)
        {
            PopupManager.instance.CloseAndOpen("lobby");
            isLogin = false;
        }           

        if(isChangeTeam)
        {
            ChangeTeam();
            isChangeTeam = false;
        }

        if(isTimeRefresh)
        {
            TimeHandler.Instance.TimeRefresh(timeData.day, timeData.isLightTime);
            isTimeRefresh = false;
        }

        while (removeSocketQueue.Count > 0)
        {
            int soc = removeSocketQueue.Dequeue();
            playerList[soc].TeamUI.SetActive(false);
            playerList[soc].SetDisable(true);
            playerList[soc].RemoveCharacter();
            playerList.Remove(soc);
        }
    }

    private void ChangeTeam()
    {
        if(changeTeamVO.user.socketId.Equals(socketId))
        {
            user.ChangeUI(changeTeamVO.user);
        }
        else if(playerList.TryGetValue(changeTeamVO.user.socketId,out Player p))
        {
            p.ChangeUI(changeTeamVO.user);
        }

        CharacterSelectPanel.Instance.InitEnable();

        List<int> selectedIdList = user.CurTeam.Equals(Team.BLUE) ? changeTeamVO.blueSelectedCharId : changeTeamVO.redSelectedCharId;

        foreach (int id in selectedIdList)
        {
            CharacterProfile profile = CharacterSelectPanel.Instance.GetCharacterProfile(id);
            profile.BtnEnabled(false);
        }

    }

    public Dictionary<int,Player> GetPlayerDic()
    {
        return playerList;
    }
    public List<Player> GetPlayerList()
    {
        return playerList.Values.ToList();
    }

    public void EnterRoom()
    {
        PopupManager.instance.ClosePopup();
        map.SetActive(true);
    }
    public void ExitRoom()
    {
        roomNum = 0;

        PlayerClear();
        map.SetActive(false);

        EventManager.OccurExitRoom();
        PopupManager.instance.CloseAndOpen("lobby");
    }

    public void GameEnd()
    {
        EventManager.OccurBackToRoom();
        print("BackToRoom 실행");
    }

    public void PlayerClear()
    {
        if(user != null)
        {
            user.StopSend();
            user.SetDisable(true);
            user = null;
        }

        foreach (int key in playerList.Keys)
        {
            playerList[key].SetDisable();
        }
        playerList.Clear();
        InfoManager.InitTeamInfoUIs();
    }
    
    public Player MakeRemotePlayer(UserVO data,CharacterSO so)
    {
        Player rpc = PoolManager.GetItem<Player>();
        InfoUI ui = InfoManager.SetInfoUI(rpc.transform, data.name);
        TeamInfoUI teamUI = InfoManager.SetTeamInfoUI(rpc, data.name);

        rpc.InitPlayer(data,ui,teamUI, true,so);
        rpc.SetTransform(data.position);

        playerList.Add(data.socketId, rpc);
        return rpc;
    }
}
