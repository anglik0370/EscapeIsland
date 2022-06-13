using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshUsers : ISetAble
{
    public static RefreshUsers Instance { get; private set; }

    private bool needUserRefresh = false;
    private bool needPosRefresh = false;

    private List<UserVO> userDataList;
    private NotLerpMoveVO data;

    public GameObject[] lights;
    public CinemachineVirtualCamera followCam;

    private Vector3 lightPos;

    private bool isOnce = false;
    public bool isTest = false;

    private Coroutine co;

    private void Awake()
    {
        lightPos = new Vector3(0, -0.76f, 0);

        PoolManager.CreatePool<LightMap>(lights[0], transform, 30);
        PoolManager.CreatePool<ShadowLight>(lights[1], transform, 30);

        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        EventManager.SubExitRoom(() =>
        {
            isOnce = false;
            isTest = false;
        });
    }

    private void Update()
    {
        if (needUserRefresh)
        {
            RefreshUser();
            needUserRefresh = false;
        }

        if(needPosRefresh)
        {
            RefreshPos();
            needPosRefresh = false;
        }
    }
    public static void SetUserRefreshData(List<UserVO> list)
    {
        lock (Instance.lockObj)
        {
            Instance.userDataList = list;
            Instance.needUserRefresh = true;
        }
    }

    public static void SetNotLerpMovedata(NotLerpMoveVO data)
    {
        lock(Instance.lockObj)
        {
            Instance.needPosRefresh = true;
            Instance.data = data;
        }
    }

    public void RefreshPos()
    {
        Init();

        if(playerList.TryGetValue(data.socketId,out Player p))
        {
            p.SetPosition(data.pos);
        }
        else if(data.socketId == user.socketId)
        {
            if (co != null)
            {
                StopCoroutine(co);
            }

            co = StartCoroutine(CoroutineHandler.EnableDampingEndFrame(GameManager.Instance.CmVCam));
            user.transform.position = data.pos;
        }
    }

    public void RefreshUser()
    {
        Init();

        if (!isOnce)
        {
            UserVO uv = userDataList.Find(x => x.socketId == socketId);
            CharacterProfile profile = CharacterSelectPanel.Instance.GetDefaultProfile();
            Player user = PoolManager.GetItem<Player>();

            InfoManager.instance.MainPlayer = user;
            InfoUI ui = InfoManager.SetInfoUI(user.transform, uv.name);
            user.InitPlayer(uv, ui, false, profile.GetSO());

            Transform lightMap = PoolManager.GetItem<LightMap>().transform;
            Transform shadowLight = PoolManager.GetItem<ShadowLight>().transform;

            lightMap.SetParent(user.transform);
            shadowLight.SetParent(user.transform);

            lightMap.localPosition = lightPos;
            shadowLight.localPosition = lightPos;

            if (isTest)
            {
                //user.isKidnapper = true; <- 아마 작동 안할 것. 서버에서 보내주는 데이터로 바뀜

                SendManager.Instance.Send("TEST_CLIENT");
            }


            NetworkManager.instance.roomNum = uv.roomNum;

            followCam.Follow = user.gameObject.transform;

            isOnce = true;
            EventManager.OccurEnterRoom(user);
        }

        foreach (UserVO uv in userDataList)
        {
            if (uv.socketId != socketId)
            {
                Player p = null;
                playerList.TryGetValue(uv.socketId, out p);

                if (p == null)
                {
                    CharacterProfile profile = CharacterSelectPanel.Instance.GetCharacterProfile(uv.charId);

                    p = NetworkManager.instance.MakeRemotePlayer(uv,profile == null ? null : profile.GetSO());

                    if (profile != null && p != null)
                    {
                        p.ChangeCharacter(profile.GetSO());
                    }
                }
                else
                {
                    p.SetTransform(uv.position);

                    p.AreaState = uv.areaState;

                    if (uv.voiceData != null && uv.voiceData.Length > 0)
                    {
                        p.PlayVoice(MicManager.Instance.GetClip(uv.voiceData));
                        print("play");
                    }
                }
            }
        }
    }
}
