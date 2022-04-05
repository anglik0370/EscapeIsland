using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class SlotManager : MonoBehaviour
{
    public static SlotManager Instance { get; private set; }

    [SerializeField]
    private ItemSlot beginSlot; //드래그 시작

    [SerializeField]
    private ItemSlot endSlot;   //드래그 끝

    [SerializeField]
    private ItemGhost ghost;

    [SerializeField]
    private bool isDraging = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if(isDraging)
        {
            ghost.SetPosition(Input.mousePosition);
        }
    }

    public void BeginDrag(ItemSlot slot)
    {
        beginSlot = slot;

        ghost.SetItem(beginSlot.GetItem());

        isDraging = true;
    }

    public void EndDrag(ItemSlot slot)
    {
        endSlot = slot;
        EndDrag();
    }

    public void EndDrag()
    {
        if(endSlot != null)
        {
            if(!beginSlot.CanDrag | !endSlot.CanDrag | !beginSlot.CanDrop | !endSlot.CanDrop)
            {
                beginSlot = null;
                endSlot = null;

                isDraging = false;

                return;
            }

            ItemSO temp = beginSlot.GetItem();

            beginSlot.SetItem(endSlot.GetItem());
            endSlot.SetItem(temp);
        }

        ghost.Init();

        beginSlot = null;
        endSlot = null;

        isDraging = false;

        return;
    }
}
