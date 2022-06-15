using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private CinemachineVirtualCamera cmVCam;
    public CinemachineVirtualCamera CmVCam => cmVCam;

    [SerializeField]
    private List<IInteractionObject> interactionObjList = new List<IInteractionObject>();
    public List<IInteractionObject> InteractionObjList => interactionObjList;

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
        for (int i = 0; i < interactionObjList.Count; i++)
        {
            if (interactionObjList[i] == null) continue;

            if (Physics2D.IsTouching(interactionObjList[i].InteractionCol, player.BodyCollider))
            {
                return interactionObjList[i];
            }
        }

        return null;
    }
}
