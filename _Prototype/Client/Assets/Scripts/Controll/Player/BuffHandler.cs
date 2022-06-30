using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffHandler : MonoBehaviour
{
    private readonly Dictionary<BuffSO, TimedBuff> _buffs = new Dictionary<BuffSO, TimedBuff>();

    private void Update()
    {
        if (_buffs.Count <= 0) return;

        foreach (TimedBuff buff in _buffs.Values.ToList())
        {
            buff.Tick(Time.deltaTime);
            if (buff.isFinished)
            {
                _buffs.Remove(buff.Buff);
            }
        }
    }

    public void AddBuff(TimedBuff buff)
    {
        if (buff is CrowdControl)
        {
            TimedBuff highDurationBuff = null;
            float maxDuration = float.MinValue;

            foreach (TimedBuff _buff in _buffs.Values.ToList())
            {
                if (_buff is CrowdControl)
                {
                    if (maxDuration < _buff.Duration)
                    {
                        maxDuration = _buff.Duration;
                        highDurationBuff = _buff;
                    }
                }
            }

            if (buff.Duration > maxDuration && highDurationBuff != null)
            {
                print($"maxDuration : {maxDuration} buffId : {buff.Buff.id}");

                highDurationBuff.End();
                _buffs.Remove(highDurationBuff.Buff);
            }
            else if (buff.Duration < maxDuration)
            {
                return;
            }
        }

        if (!_buffs.ContainsKey(buff.Buff))
        {
            _buffs.Add(buff.Buff, buff);
        }

        _buffs[buff.Buff].Activate();
    }

    public void RemoveAllDebuff()
    {
        foreach (TimedBuff buff in _buffs.Values.ToList())
        {
            if (buff.Buff.isBuffed) continue;

            buff.End();
            _buffs.Remove(buff.Buff);
        }
    }
}
