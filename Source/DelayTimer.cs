using System;
using UnityEngine;

public class DelayTimer : Timer
{
    public DelayTimer(bool isPersistence, float duration, Action onComplete, Action<float> onUpdate, bool usesRealTime, MonoBehaviour autoDestroyOwner)
        : base(isPersistence, duration, onComplete, onUpdate, usesRealTime, autoDestroyOwner)
    {
    }
    
    protected override void Update()
    {
        if(!CheckUpdate()) return;

        if (_onUpdate != null)
            SafeCall(_onUpdate, GetTimeElapsed());

        if (GetWorldTime() >= GetFireTime())
        {
            SafeCall(_onComplete);
            isCompleted = true;
        }
    }
    
    private float GetFireTime()
    {
        return _startTime + duration;
    }
}