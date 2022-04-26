using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConverter : MonoBehaviour, IInteractionObject
{
    private SpriteRenderer sr;

    [SerializeField]
    private InteractionSO lobbyHandlerSO;
    public InteractionSO LobbyHandlerSO => lobbyHandlerSO;

    [SerializeField]
    private InteractionSO ingameHandlerSO;
    public InteractionSO InGameHandlerSO => ingameHandlerSO;

    public Action LobbyCallback => () => { };
    public Action IngameCallback => () => ConvertPanel.Instance.Open(this);

    public bool CanInteraction => true;

    public int id;

    private ItemSO beforeItem;
    public ItemSO BeforeItem => beforeItem;

    private ItemSO afterItem;
    public ItemSO AfterItem => afterItem;

    [SerializeField]
    private Transform interactionTrm;

    [SerializeField]
    private List<ConvertRecipeSO> convertRecipeList;

    private Dictionary<ItemSO, ItemSO> convertRecipeDic;

    [SerializeField]
    private bool isConverting = false; //변환 중인지
    public bool IsConverting => isConverting;

    [SerializeField]
    private float convertingTime = 5f; //재련하는데 걸리는 시간
    public float ConvertingTime => convertingTime;

    [SerializeField]
    private float remainTime; //남은 재련시간
    public float RemainTime => remainTime;

    private void Awake() 
    {
        sr = GetComponent<SpriteRenderer>();

        convertRecipeDic = new Dictionary<ItemSO, ItemSO>();
    }

    private void Start()
    {
        foreach (ConvertRecipeSO so in convertRecipeList)
        {
            convertRecipeDic.Add(so.beforeItem, so.afterItem);
        }

        EventManager.SubGameStart(p =>
        {
            beforeItem = null;
            afterItem = null;

            isConverting = false;

            remainTime = 0f;
        });
    }

    private void Update() 
    {
        if(isConverting)
        {
            remainTime -= Time.deltaTime;

            if(remainTime <= 0)
            {
                ConvertingEnd();
            }
        }
    }

    public Transform GetTrm()
    {
        return transform;
    }

    public Transform GetInteractionTrm()
    {
        return interactionTrm;
    }

    public Sprite GetSprite()
    {
        return sr.sprite;
    }

    public bool GetFlipX()
    {
        return sr.flipX;
    }

    public void SetAfterItem(ItemSO item)
    {
        this.afterItem = item;
    }

    public void ConvertingStart(ItemSO beforeItem)
    {
        //제련 시작 시 (제련 전 아이템을 넣었을 때)
        //필요한것 - 제련소 id, 넣은 아이템SO
        isConverting = true;
        remainTime = convertingTime;

        this.beforeItem = beforeItem;

        if(ConvertPanel.Instance.IsOpenRefinery(this))
        {
            ConvertPanel.Instance.UpdateUIs();
        }
    }

    public void ConvertingReset()
    {
        //제련이 다 되기 전에 뺐을때
        remainTime = 0f;
        isConverting = false;
        beforeItem = null;

        if(ConvertPanel.Instance.IsOpenRefinery(this))
        {
            ConvertPanel.Instance.UpdateUIs();
            ConvertPanel.Instance.ResetUIs();
        }
    }

    public void ConvertingEnd()
    {
        remainTime = 0f;
        isConverting = false;

        //제련 끝나면 해줄일
        afterItem = FindAfterItem(beforeItem);
        beforeItem = null;

        if(ConvertPanel.Instance.IsOpenRefinery(this))
        {
            ConvertPanel.Instance.UpdateUIs();
        }
    }

    public void TakeIAfterItem()
    {
        //아이템을 가져갔을 때
        afterItem = null;

        if(ConvertPanel.Instance.IsOpenRefinery(this))
        {
            ConvertPanel.Instance.UpdateUIs();
            ConvertPanel.Instance.ResetUIs();
        }
    }

    public ItemSO FindAfterItem(ItemSO beforeItem)
    {
        return convertRecipeDic[beforeItem];
    }

    public bool IsCanConvert(ItemSO so)
    {
        return convertRecipeList.Find(x => x == so) != null;
    }
}
