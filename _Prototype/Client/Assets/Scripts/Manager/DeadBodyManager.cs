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

        //PoolManager.CreatePool<DeadBody>(deadBodyPrefab.gameObject, transform, 5);
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

        EventManager.SubExitRoom(() =>
        {
            ClearDeadBody();
        });
    }

    public void MakeDeadbody(Vector3 pos, bool isFlip, CharacterSO characterSO)
    {
        DeadBody deadBody = PoolManager.GetItem<DeadBody>();
        deadBody.GetTrm().position = pos;

        deadBody.Init(pos, isFlip, characterSO);

        if(!deadBodyList.Contains(deadBody))
            deadBodyList.Add(deadBody);
        GameManager.Instance.AddInteractionObj(deadBody);
    }

    public void ClearDeadBody()
    {
        if (deadBodyList.Count == 0) return;

        foreach (DeadBody deadBody in deadBodyList)
        {
            GameManager.Instance.RemoveInteractionObj(deadBody);

            if (!deadBody.gameObject.activeSelf)
            {
                continue;
            }

            deadBody.gameObject.SetActive(false);
        }

        deadBodyList.Clear();
    }
}
