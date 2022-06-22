using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MSWithObj : MonoBehaviour, IStorageMission
{
    [SerializeField]
    private Team team;
    public Team Team => team;

    [SerializeField]
    private ItemSO storageItem;
    public ItemSO StorageItem => storageItem;

    private CanvasGroup cvs;
    public CanvasGroup Cvs => cvs;

    [SerializeField]
    private int maxPanelCount;

    [SerializeField]
    private int nextCloseCount;

    [SerializeField]
    private List<MSSlot> slotList;

    [SerializeField]
    private List<MSObject> objList = new List<MSObject>();

    private List<MSObject> redObjList = new List<MSObject>();
    private List<MSObject> blueObjList = new List<MSObject>();

    [SerializeField]
    private bool isAutoClosed = false;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
        slotList = GetComponentsInChildren<MSSlot>().ToList();
    }

    private void Start()
    {
        objList.ForEach(x => x.Disable());

        redObjList = objList.FindAll(x => x.Team == Team.RED);
        blueObjList = objList.FindAll(x => x.Team == Team.BLUE);

        EventManager.SubGameStart(p =>
        {
            isAutoClosed = false;

            int curItemCount = StorageManager.Instance.FindItemAmount(false, team, storageItem).amount;
            int maxItemCount = StorageManager.Instance.FindItemAmount(true, team, storageItem).amount;

            maxItemCount = StorageManager.Instance.FindNeedItemAmount(storageItem);
            maxPanelCount = maxItemCount / redObjList.Count; //나머지 안남게 세팅 부탁

            nextCloseCount = 0;

            if (maxPanelCount > 1)
            {
                for (int i = 0; i < slotList.Count; i++)
                {
                    slotList[i].DisableSlot();
                }

                for (int i = 0; i < maxPanelCount; i++)
                {
                    slotList[i].EnableSlot();
                }
            }

            for (int i = 0; i < slotList.Count; i++)
            {
                slotList[i].DisableImg();
            }

            for (int i = 0; i < objList.Count; i++)
            {
                objList[i].Disable();
            }

            UpdateCurItem();
        });
    }

    public void Open()
    {
        UpdateCurItem();
    }

    public void Close()
    {

    }

    public void SetTeam(Team team)
    {
        this.team = team;
    }

    public void UpdateCurItem()
    {
        int curItemCount = StorageManager.Instance.FindItemAmount(false, team, storageItem).amount;
        int maxItemCount = StorageManager.Instance.FindItemAmount(true, team, storageItem).amount;

        if (curItemCount >= maxItemCount)
        {
            MissionPanel.Instance.Close();
        }

        nextCloseCount = 0;

        if (team == Team.RED)
        {
            for (int i = 0; i < redObjList.Count; i++)
            {
                if(!redObjList[i].IsEmpty)
                {
                    nextCloseCount += maxPanelCount;
                }
            }
        }
        else if (team == Team.BLUE)
        {
            for (int i = 0; i < blueObjList.Count; i++)
            {
                if (!blueObjList[i].IsEmpty)
                {
                    nextCloseCount += maxPanelCount;
                }
            }
        }

        nextCloseCount += maxPanelCount;

        if (curItemCount >= nextCloseCount)
        {
            MissionPanel.Instance.Close();
            nextCloseCount += maxPanelCount;
        }

        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].DisableImg();
        }

        int objCount = 0;

        for (int i = curItemCount - maxPanelCount; i >= 0; i -= maxPanelCount)
        {
            objCount++;
        }

        if(objCount > 0)
        {
            if(team == Team.RED)
            {
                for (int i = 0; i < objCount; i++)
                {
                    redObjList[i].Enable();
                }
            }
            else if(team == Team.BLUE)
            {
                for (int i = 0; i < objCount; i++)
                {
                    blueObjList[i].Enable();
                }
            }
        }

        if(maxPanelCount > 1)
        {
            int itemCount = curItemCount % maxPanelCount;

            for (int i = 0; i < itemCount; i++)
            {
                slotList[i].EnableImg();
            }
        }
        else
        {
            for (int i = 0; i < curItemCount; i++)
            {
                slotList[i].EnableImg();
            }
        }
    }
}
