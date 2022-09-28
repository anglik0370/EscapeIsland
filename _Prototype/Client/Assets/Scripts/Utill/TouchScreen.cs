using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchScreen : MonoBehaviour, IPointerDownHandler
{
    private Action TouchCallback = () => { };

    public void SubTouchEvent(Action Callback)
    {
        TouchCallback += Callback;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        TouchCallback?.Invoke();
    }
}
