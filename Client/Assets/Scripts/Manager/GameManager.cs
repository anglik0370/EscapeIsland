using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private Transform sandInteractionParentTrm;
    private List<Collider2D> sandInteractionColList;
    [SerializeField]
    private ItemSpawner sandSpawner;

    [SerializeField]
    private Transform seaInteractionColParentTrm;
    private List<Collider2D> seaInteractionColList;
    [SerializeField]
    private ItemSpawner waterSpawner;

    [SerializeField]
    private List<IInteractionObject> interactionObjList = new List<IInteractionObject>();
    public List<IInteractionObject> InteractionObjList => interactionObjList;

    [SerializeField]
    private List<GameObject> objList = new List<GameObject>();

    [SerializeField]
    private Transform indoorColliderParentTrm;
    private List<Collider2D> indoorColList = new List<Collider2D>();

    public bool IsPanelOpen { get; set; }

    private Player player;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        sandInteractionColList = sandInteractionParentTrm.GetComponentsInChildren<Collider2D>().ToList();
        seaInteractionColList = seaInteractionColParentTrm.GetComponentsInChildren<Collider2D>().ToList();
        indoorColList = indoorColliderParentTrm.GetComponentsInChildren<Collider2D>().ToList();
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            player = p;

            IsPanelOpen = false;
        });

        EventManager.SubGameStart(p =>
        {
            IsPanelOpen = false;
        });

        EventManager.SubExitRoom(() =>
        {
            interactionObjList.Clear();
        });
    }

    private void Update()
    {
        objList.Clear();

        for (int i = 0; i < interactionObjList.Count; i++)
        {
            if (interactionObjList[i].GetTrm().gameObject == null) continue;

            objList.Add(interactionObjList[i].GetTrm().gameObject);
        }
    }

    public void AddInteractionObj(IInteractionObject interactionObject)
    {
        interactionObjList.Add(interactionObject);
    }

    public void RemoveInteractionObj(IInteractionObject interactionObject)
    {
        interactionObjList.Remove(interactionObject);
    }

    public IInteractionObject GetProximateObject()
    {
        for (int i = 0; i < sandInteractionColList.Count; i++)
        {
            if (Physics2D.IsTouching(sandInteractionColList[i], player.FootCollider))
            {
                return sandSpawner;
            }
        }

        for (int i = 0; i < seaInteractionColList.Count; i++)
        {
            if(Physics2D.IsTouching(seaInteractionColList[i], player.FootCollider))
            {
                return waterSpawner;
            }
        }

        IInteractionObject proximateObj = interactionObjList[0];

        for (int i = 0; i < interactionObjList.Count; i++)
        {
            if (!interactionObjList[i].CanInteraction) continue;

            if (Vector2.Distance(player.GetTrm().position, interactionObjList[i].GetInteractionTrm().position) <
                Vector2.Distance(player.GetTrm().position, proximateObj.GetInteractionTrm().position))
            {
                proximateObj = interactionObjList[i];
            }
        }

        if(player.CheckInRange(proximateObj))
        {
            return proximateObj;
        }
        else
        {
            return null;
        }
    }
}
