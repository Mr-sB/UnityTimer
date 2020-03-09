using System;
using UnityEngine;

public class DelayFrameTimer : Timer
{
    private readonly int _frame;

    protected override float GetWorldTime()
    {
        return Time.frameCount;
    }

    public override float GetTimeElapsed()
    {
        if (isCompleted)
        {
            return duration;
        }

        return _timeElapsedBeforeCancel ??
               _timeElapsedBeforePause ??
               _timeElapsedBeforeAutoDestroy ??
               GetWorldTime() - _startTime;
    }

    public DelayFrameTimer(bool isPersistence, int frame, Action onComplete, Action<float> onUpdate, MonoBehaviour autoDestroyOwner)
        : base(isPersistence, frame, onComplete, onUpdate, true, autoDestroyOwner)
    {
        _frame = frame;
    }

    protected override void Update()
    {
        if(!CheckUpdate()) return;

        SafeCall(_onUpdate, GetTimeElapsed());
        //avoid float precision cause equal judge fail
        if (Mathf.Abs(GetWorldTime() - GetFireTime()) <= 1e-4)
        {
            SafeCall(_onComplete);
            isCompleted = true;
        }
    }
}