using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGhost : MonoBehaviour
{
    private CanvasGroup cvs;
    private Image image;

    [SerializeField]
    private ItemSO item;

    private void Awake() 
    {
        image = GetComponent<Image>();
        cvs = GetComponent<CanvasGroup>();
        Init();
    }

    public void Init()
    {
        item = null;
        cvs.alpha = 0f;
        cvs.interactable = false;
        cvs.blocksRaycasts = false;
    }

    public void SetPosition(Vector3 pos)
    {
        pos.z = 10.0f;
        
        transform.position = Camera.main.ScreenToWorldPoint(pos);
    }

    public void SetItem(ItemSO item)
    {
        this.item = item;

        if(item == null)
        {
            image.sprite = null;
        }
        else
        {
            image.sprite = item.itemSprite;
            cvs.alpha = 1f;
        }
    }

    public ItemSO GetItem()
    {
        return this.item;
    }
}
