using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArsonManager : MonoBehaviour
{
    public static ArsonManager Instance { get; private set; }

    [SerializeField]
    private List<ItemSO> waterList;

    [SerializeField]
    private ArsonSlot redSlot;
    [SerializeField]
    private ArsonSlot blueSlot;

    [SerializeField]
    private Transform arsonParent;

    public bool isArson = false;
    public bool isBlue = false;

    private void Awake()
    {
        Instance = this;

        EventManager.SubGameOver(goc => SlotActive(false));
        EventManager.SubExitRoom(() => SlotActive(false));
    }

    private void Start()
    {
        SlotActive(false);
    }

    public void SlotActive(bool active)
    {
        redSlot.SetArson(active);
        blueSlot.SetArson(active);
        isArson = false;
    }

    public void StartArson()
    {
        isArson = true;
        if (isBlue)
        {
            redSlot.SetArson(true);
        }
        else
        {
            blueSlot.SetArson(true);
        }

        EyesightManager.Instance.ChangeVisibleObjects(NetworkManager.instance.User.AreaState);
    }

    public bool CanExtinguish(ItemSO so)
    {
        return waterList.Find(s => s == so) != null;
    }
}
