using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMsgHandler
{
    public void HandleMsg(string payload);
}

public abstract class IMsgHandler<T> : MonoBehaviour,IMsgHandler where T : ISetAble
{
    protected bool once = false;
    protected T generic = null;
    public virtual void HandleMsg(string payload) 
    {
        if(!once)
        {
            generic = NetworkManager.instance.FindSetDataScript<T>();
            once = true;
        }
    }
}
