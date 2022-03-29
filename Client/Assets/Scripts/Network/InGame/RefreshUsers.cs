using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshUsers : ISetAble
{
    private bool needUserRefresh = false;
    private List<UserVO> userDataList;

    public GameObject[] lights;
    public CinemachineVirtualCamera followCam;

    private bool isOnce = false;
    public bool isTest = false;


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
        Init();

        foreach (UserVO uv in userDataList)
        {
            CharacterProfile profile = CharacterSelectPanel.Instance.GetDefaultProfile();
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
                if (!isOnce)
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
                        //user.isKidnapper = true; <- 아마 작동 안할 것. 서버에서 보내주는 데이터로 바뀜

                        SendManager.Instance.Send("TEST_CLIENT");
                    }


                    NetworkManager.instance.roomNum = uv.roomNum;

                    followCam.Follow = user.gameObject.transform;

                    isOnce = true;
                    EventManager.OccurEnterRoom(user);
                }
            }

        }
    }
}
