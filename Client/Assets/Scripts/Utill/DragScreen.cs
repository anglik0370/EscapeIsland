using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragScreen : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private Vector2 beginPoint;
    [SerializeField]
    private Vector2 endPoint;

    private Action<Vector2, Vector2> OnDragEnd = (begin, end) => { };

    public void SubOnEndDrag(Action<Vector2, Vector2> Callback)
    {
        OnDragEnd += Callback;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginPoint = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endPoint = eventData.position;

        OnDragEnd?.Invoke(beginPoint, endPoint);
    }
}
