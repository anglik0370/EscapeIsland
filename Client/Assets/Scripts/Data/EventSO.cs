using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/Event", fileName = "New Event")]
public class EventSO : ScriptableObject
{
    private List<EventListener> eventListenerList = new List<EventListener>();

    public void Register(EventListener listener)
    {
        eventListenerList.Add(listener);
    }

    public void UnRegister(EventListener listener)
    {
        eventListenerList.Remove(listener);
    }

    public void Occurred()
    {
        for(int i = 0; i < eventListenerList.Count; i++)
        {
            eventListenerList[i].OnEventOccurs();
        }
    }
}
