using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBodyManager : MonoBehaviour
{
    public static DeadBodyManager Instance { get; private set; }

    public List<DeadBody> deadBodyList = new List<DeadBody>();

    [SerializeField]
    private DeadBody deadBodyPrefab;

    private Player player;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        PoolManager.CreatePool<DeadBody>(deadBodyPrefab.gameObject, transform, 5);
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            player = p;
        });

        EventManager.SubBackToRoom(() =>
        {
            ClearDeadBody();
        });

        EventManager.SubStartMeet(mt =>
        {
            ClearDeadBody();
        });
    }

    public bool FindProximateDeadBody(out DeadBody temp)
    {
        DeadBody deadBody = null;

        for (int i = 0; i < deadBodyList.Count; i++)
        {
            //��ȣ�ۿ���� �ȿ� �ִ��� üũ
            if (Vector2.Distance(player.GetTrm().position, deadBodyList[i].transform.position) <= player.range)
            {
                if (deadBody == null)
                {
                    //������ �ϳ� �־��ְ�
                    deadBody = deadBodyList[i];
                }
                else
                {
                    //������ �Ÿ���
                    if (Vector2.Distance(player.GetTrm().position, deadBody.transform.position) >
                        Vector2.Distance(player.GetTrm().position, deadBodyList[i].transform.position))
                    {
                        deadBody = deadBodyList[i];
                    }
                }
            }
        }

        temp = deadBody;

        return temp != null;
    }

    public void ReportProximateDeadbody()
    {
        FindProximateDeadBody(out DeadBody deadBody);

        if (deadBody == null) return;

        deadBody.Report();
    }

    public void ClearDeadBody()
    {
        if (deadBodyList.Count == 0) return;

        foreach (DeadBody deadBody in deadBodyList)
        {
            if (!deadBody.gameObject.activeSelf)
            {
                continue;
            }

            deadBody.gameObject.SetActive(false);
        }

        deadBodyList.Clear();
    }
}
