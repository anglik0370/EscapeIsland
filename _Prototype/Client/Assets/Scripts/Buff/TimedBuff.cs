using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimedBuff
{
    protected float duration;
    public float Duration => duration;
    protected int effectStacks;
    public BuffSO Buff { get; }
    protected readonly GameObject obj;
    public bool isFinished;

    public TimedBuff(BuffSO buff,GameObject obj)
    {
        this.Buff = buff;
        this.obj = obj;
        duration = buff.duration;
    }

    public void Tick(float delta)
    {
        duration -= delta;
        if (duration <= 0)
        {
            End();
            isFinished = true;
        } 
    }

    public void Activate()
    {
        if(Buff.isEffectStacked || effectStacks.Equals(0))
        {
            ApplyEffect();
            effectStacks++;
        }

        if (Buff.isDurationStacked)
        {
            duration = duration <= 0 ? Buff.duration : duration + Buff.duration;
        }
        else
        {
            duration = Buff.duration;
        }
    }

    protected abstract void ApplyEffect();
    public abstract void End();
}
