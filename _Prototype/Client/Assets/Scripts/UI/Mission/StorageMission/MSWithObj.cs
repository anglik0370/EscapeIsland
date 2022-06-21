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
    private List<MSSlot> slotList;

    [SerializeField]
    private List<MSObject> objList = new List<MSObject>();

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
        slotList = GetComponentsInChildren<MSSlot>().ToList();
    }

    private void Start()
    {
        objList.ForEach(x => x.Disable());

        EventManager.SubGameStart(p =>
        {
            try
            {
                int curItemCount = StorageManager.Instance.FindItemAmount(false, team, storageItem).amount;
                int maxItemCount = StorageManager.Instance.FindItemAmount(true, team, storageItem).amount;

                maxItemCount = StorageManager.Instance.FindNeedItemAmount(storageItem);
                maxPanelCount = maxItemCount / objList.Count; //나머지 안남게 세팅 부탁

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
            }
            catch
            {
                print(gameObject.name);
            }
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

        if (maxPanelCount > 1)
        {
            int tmpItemCnt = curItemCount;

            while (tmpItemCnt > 0)
            {
                tmpItemCnt -= maxPanelCount;
            }

            if (tmpItemCnt == 0)
            {
                MissionPanel.Instance.Close();
            }
        }
        else
        {
            if (curItemCount >= maxItemCount)
            {
                MissionPanel.Instance.Close();
            }
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
            for (int i = 0; i < objCount; i++)
            {
                objList[i].Enable();
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
