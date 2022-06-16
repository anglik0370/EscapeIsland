using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MSSlot : ItemSlot
{
    [SerializeField]
    private Image guideImage;

    private IStorageMission mission;
    public IStorageMission Mission => mission;

    public new bool IsEmpty => image.color == UtilClass.limpidityColor;
    public bool IsEnable => guideImage.color == UtilClass.guideColor;

    protected override void Awake()
    {
        mission = GetComponentInParent<IStorageMission>();

        image = GetComponent<Image>();
        image.color = UtilClass.limpidityColor;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (!IsEnable) return;
        //드롭 오브젝트에서 발생
        SlotManager.Instance.EndDrag(this);
    }

    public void EnableImg()
    {
        image.color = UtilClass.opacityColor;
    }

    public void DisableImg()
    {
        image.color = UtilClass.limpidityColor;
    }
    
    public void EnableSlot()
    {
        guideImage.color = UtilClass.guideColor;
    }

    public void DisableSlot()
    {
        guideImage.color = UtilClass.limpidityColor;
    }
}
