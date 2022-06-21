using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MSOnly : MonoBehaviour, IStorageMission
{
    [SerializeField]
    private Transform slotParentTrm;
    [SerializeField]
    private Transform guideParentTrm;

    [SerializeField]
    private MSSlot slotPrefab;
    [SerializeField]
    private Image guideImgPrefab;

    [SerializeField]
    private Team team;
    public Team Team => team;

    [SerializeField]
    private ItemSO storageItem;
    public ItemSO StorageItem => storageItem;

    private CanvasGroup cvs;
    public CanvasGroup Cvs => cvs;

    [SerializeField]
    private List<MSSlot> slotList;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();
        slotList = new List<MSSlot>();
    }

    private void Start()
    {
        EventManager.SubGameStart(p =>
        {
            try
            {
                int maxItemCount = StorageManager.Instance.FindItemAmount(true, team, storageItem).amount;

                for (int i = 0; i < slotList.Count; i++)
                {
                    slotList[i].gameObject.SetActive(true);
                }

                if (maxItemCount > slotList.Count)
                {
                    int slotCount = slotList.Count;

                    //아이템 갯수보다 슬롯이 적으면
                    for (int i = slotCount; i < maxItemCount; i++)
                    {
                        MSSlot tmpSlot = Instantiate(slotPrefab, slotParentTrm);
                        Image tmpGuideImg = Instantiate(guideImgPrefab, guideParentTrm);

                        tmpSlot.SetGuideImg(tmpGuideImg);

                        slotList.Add(tmpSlot);
                    }
                }
                else if (maxItemCount < slotList.Count)
                {
                    for (int i = maxItemCount - 1; i < slotList.Count; i++)
                    {
                        slotList[i].gameObject.SetActive(false);
                    }
                }

                for (int i = 0; i < slotList.Count; i++)
                {
                    slotList[i].EnableSlot();
                }

                UpdateCurItem();
            }
            catch (System.Exception)
            {
                print(gameObject.name);
            }
        });
    }

    public void Close()
    {
        
    }

    public void Open()
    {
        int curItemCount = StorageManager.Instance.FindItemAmount(false, team, storageItem).amount;
        int maxItemCount = StorageManager.Instance.FindItemAmount(true, team, storageItem).amount;

        if (curItemCount >= maxItemCount) return;

        UpdateCurItem();
    }

    public void SetTeam(Team team)
    {
        this.team = team;
    }

    public void AddCurItem()
    {
        StorageManager.Instance.FindItemAmount(false, team, storageItem).amount++;
    }

    public void UpdateCurItem()
    {
        int curItemCount = StorageManager.Instance.FindItemAmount(false, team, storageItem).amount;

        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].DisableImg();
        }

        for (int i = 0; i < curItemCount; i++)
        {
            slotList[i].EnableImg();
        }
    }
}
