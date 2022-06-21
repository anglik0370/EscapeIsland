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

    private UserVO teamChanger;
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

        EventManager.SubBackToRoom(() =>
        {
            InitPlayers();
        });
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

    public static void SetChangeTeam(UserVO uv)
    {
        lock(instance.lockObj)
        {
            instance.teamChanger = uv;
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
        if(teamChanger.socketId.Equals(socketId))
        {
            user.ChangeUI(teamChanger);
        }
        else if(playerList.TryGetValue(teamChanger.socketId,out Player p))
        {
            p.ChangeUI(teamChanger);
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

    public bool GetPlayerDie(int socId)
    {
        Player p = null;

        playerList.TryGetValue(socId, out p);

        return p != null && p.isDie;
    }

    public void InitPlayers()
    {
        user.InitPlayer();

        foreach (int idx in playerList.Keys)
        {
            playerList[idx].InitPlayer();
        }

        PlayerEnable(true);
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
    
    public void PlayerEnable(bool isEnd = false)
    {
        if (!user.isDie && !isEnd) return;

        foreach (int key in playerList.Keys)
        {
            if(!playerList[key].gameObject.activeSelf)
            {
                playerList[key].SetEnable();
            }
        }
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
