using System;
using UnityEngine;

public class LoopTimer : Timer
{
    private readonly bool _executeOnStart;
    private readonly int? _loopCount;
    private readonly Action _onFinished;
    private int _timer;
    
    public LoopTimer(bool isPersistence, bool executeOnStart, float interval, Action onComplete, Action<float> onUpdate, bool usesRealTime, MonoBehaviour autoDestroyOwner)
        : base(isPersistence, interval, onComplete, onUpdate, usesRealTime, autoDestroyOwner)
    {
        _executeOnStart = executeOnStart;
        if (_executeOnStart)
            OnComplete();
    }
    
    public LoopTimer(bool isPersistence, bool executeOnStart, float interval, int loopCount, Action onComplete, Action<float> onUpdate, Action onFinished, bool usesRealTime, MonoBehaviour autoDestroyOwner)
        : base(isPersistence, interval, onComplete, onUpdate, usesRealTime, autoDestroyOwner)
    {
        _executeOnStart = executeOnStart;
        _loopCount = loopCount;
        _onFinished = onFinished;
        if (_executeOnStart)
            OnComplete();
    }

    protected override void OnRestart()
    {
        _timer = 0;
        if (_executeOnStart)
            OnComplete();
    }

    protected override void Update()
    {
        if(!CheckUpdate()) return;

        if (_onUpdate != null)
            SafeCall(_onUpdate, GetTimeElapsed());

        var timeDifference = GetWorldTime() - GetFireTime();
        if (timeDifference >= 0)
        {
            OnComplete();
            if(!isCompleted)
                _startTime = GetWorldTime() - timeDifference;//Avoid time error accumulation
        }
    }

    private void OnComplete()
    {
        if (_loopCount.HasValue)
        {
            _timer++;
            SafeCall(_onComplete);
            if (_timer >= _loopCount.Value)
            {
                SafeCall(_onFinished);
                isCompleted = true;
            }
        }
        else
            SafeCall(_onComplete);
    }
}