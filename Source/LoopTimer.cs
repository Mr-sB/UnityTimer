using System;
using UnityEngine;

public class LoopTimer : Timer
{
    private readonly bool _executeOnStart;
    private readonly int? _loopCount;
    private readonly Action _onFinished;
    private int _timer;
    
    public LoopTimer(bool isPersistence, bool executeOnStart, float duration, Action onComplete, Action<float> onUpdate, bool usesRealTime, MonoBehaviour autoDestroyOwner)
        : base(isPersistence, duration, onComplete, onUpdate, usesRealTime, autoDestroyOwner)
    {
        _executeOnStart = executeOnStart;
        if (_executeOnStart)
            OnComplete();
    }
    
    public LoopTimer(bool isPersistence, bool executeOnStart, float duration, int loopCount, Action onComplete, Action<float> onUpdate, Action onFinished, bool usesRealTime, MonoBehaviour autoDestroyOwner)
        : base(isPersistence, duration, onComplete, onUpdate, usesRealTime, autoDestroyOwner)
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

        if (GetWorldTime() >= GetFireTime())
        {
            OnComplete();
            if(!isCompleted)
                _startTime = GetWorldTime();
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
                isCompleted = true;
                SafeCall(_onFinished);
            }
        }
        else
        {
            SafeCall(_onComplete);
        }
    }
    
    private float GetFireTime()
    {
        return _startTime + duration;
    }
}