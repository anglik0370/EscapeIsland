using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private List<IInteractionObject> interactionObjList = new List<IInteractionObject>();
    public List<IInteractionObject> InteractionObjList => interactionObjList;

    [SerializeField]
    private List<GameObject> objList = new List<GameObject>();

    public bool IsPanelOpen { get; set; }

    private Player player;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            player = p;
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
