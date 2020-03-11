using System;
using UnityEngine;

public class DelayFrameTimer : Timer
{
    protected override float GetWorldTime()
    {
        return Time.frameCount;
    }

    public DelayFrameTimer(bool isPersistence, int frame, Action onComplete, Action<float> onUpdate, MonoBehaviour autoDestroyOwner)
        : base(isPersistence, frame, onComplete, onUpdate, true, autoDestroyOwner)
    {
    }

    protected override void Update()
    {
        if(!CheckUpdate()) return;

        SafeCall(_onUpdate, GetTimeElapsed());
        //minus 1e-4 to avoid float precision cause equal judge fail
        if (GetWorldTime() >= GetFireTime() - 1e-4)
        {
            SafeCall(_onComplete);
            isCompleted = true;
        }
    }
}