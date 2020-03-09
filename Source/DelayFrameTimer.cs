using System;
using UnityEngine;

public class DelayFrameTimer : Timer
{
    private readonly int _frame;
    private int _frameCount;

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

    protected override void OnRestart()
    {
        _frameCount = 0;
    }

    protected override void Update()
    {
        if(!CheckUpdate()) return;

        _frameCount++;
        SafeCall(_onUpdate, GetTimeElapsed());
        if (_frameCount >= _frame)
        {
            SafeCall(_onComplete);
            isCompleted = true;
        }
    }
}