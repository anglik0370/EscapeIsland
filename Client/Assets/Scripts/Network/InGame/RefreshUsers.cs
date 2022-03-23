using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshUsers : MonoBehaviour,ISetAble
{
    private bool needUserRefresh = false;
    private List<UserVO> userDataList;
    private Dictionary<int, Player> playerList;

    public GameObject[] lights;
    public CinemachineVirtualCamera followCam;

    private object lockObj = new object();

    private bool once = false;
    public bool isTest = false;

    private int socketId;

    private void Start()
    {
        EventManager.SubExitRoom(() =>
        {
            once = false;
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
    }
    public void SetUserRefreshData(List<UserVO> list)
    {
        lock (lockObj)
        {
            userDataList = list;
            needUserRefresh = true;
        }
    }

    public void RefreshUser()
    {
        playerList = NetworkManager.instance.GetPlayerDic();

        if(!once)
        {
            socketId = NetworkManager.instance.socketId;
        }

        foreach (UserVO uv in userDataList)
        {
            CharacterProfile profile = CharacterSelectPanel.Instance.GetNotSelectedProfile();
            if (uv.socketId != socketId)
            {
                Player p = null;
                playerList.TryGetValue(uv.socketId, out p);

                if (p == null)
                {
                    NetworkManager.instance.MakeRemotePlayer(uv, profile.GetSO());
                }
                else
                {

                    p.SetTransform(uv.position);
                }
            }
            else
            {
                if (!once)
                {
                    Player user = PoolManager.GetItem<Player>();
                    InfoUI ui = InfoManager.SetInfoUI(user.transform, uv.name);
                    user.InitPlayer(uv, ui, false, profile.GetSO());

                    for (int i = 0; i < lights.Length; i++)
                    {
                        GameObject obj = Instantiate(lights[i], user.transform);
                        obj.transform.localPosition = Vector3.zero;
                    }

                    if (isTest)
                    {
                        user.isKidnapper = true;
                        DataVO dataVO = new DataVO("TEST_CLIENT", null);

                        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
                    }


                    NetworkManager.instance.roomNum = uv.roomNum;

                    followCam.Follow = user.gameObject.transform;

                    once = true;
                    EventManager.OccurEnterRoom(user);
                }
            }

        }
    }
}
