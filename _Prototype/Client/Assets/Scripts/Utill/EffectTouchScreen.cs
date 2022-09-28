using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EffectTouchScreen : TouchScreen
{
    private Action<Vector2> TouchCallback = (pos) => { };

    public void SubTouchEvent(Action<Vector2> Callback)
    {
        TouchCallback += Callback;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        TouchCallback?.Invoke(eventData.position);
    }
}
