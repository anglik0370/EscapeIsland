using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    public EventSO gEvent;

    public UnityEvent respons = new UnityEvent();

    private void OnEnable() 
    {
        gEvent.Register(this);
    }

    private void OnDisbale()
    {
        gEvent.UnRegister(this);
    }

    public void OnEventOccurs()
    {
        Debug.LogError("경고 이 코드는 더 이상 사용되지 않습니다");
        respons.Invoke();
    }
}
