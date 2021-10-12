using System;
using GameUtil;
using UnityEngine;

/// <summary>
/// Contains extension methods related to <see cref="Timer"/>s.
/// </summary>
public static class TimerExtensions
{
    public static DelayTimer DelayAction(this Component behaviour, float duration, Action onComplete, Action<float> onUpdate = null, bool useRealTime = false)
    {
        return Timer.DelayAction(duration, onComplete, onUpdate, useRealTime, behaviour);
    }
    
    public static DelayFrameTimer DelayFrameAction(this Component behaviour, int frame, Action onComplete, Action<float> onUpdate = null)
    {
        return Timer.DelayFrameAction(frame, onComplete, onUpdate, behaviour);
    }
    
    public static LoopTimer LoopAction(this Component behaviour, float interval, Action onComplete, Action<float> onUpdate = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.LoopAction(interval, onComplete, onUpdate, useRealTime, executeOnStart, behaviour);
    }
    
    public static LoopUntilTimer LoopUntilAction(this Component behaviour, float interval, Func<LoopUntilTimer, bool> loopUntil, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.LoopUntilAction(interval, loopUntil, onComplete, onUpdate, onFinished, useRealTime, executeOnStart, behaviour);
    }
    
    public static LoopCountTimer LoopCountAction(this Component behaviour, float interval, int count, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.LoopCountAction(interval, count, onComplete, onUpdate, onFinished, useRealTime, executeOnStart, behaviour);
    }
    
    //Persistence
    public static DelayTimer PersistenceDelayAction(this Component behaviour, float duration, Action onComplete, Action<float> onUpdate = null, bool useRealTime = false)
    {
        return Timer.PersistenceDelayAction(duration, onComplete, onUpdate, useRealTime, behaviour);
    }
    
    public static DelayFrameTimer PersistenceDelayFrameAction(this Component behaviour, int frame, Action onComplete, Action<float> onUpdate = null)
    {
        return Timer.PersistenceDelayFrameAction(frame, onComplete, onUpdate, behaviour);
    }
    
    public static LoopTimer PersistenceLoopAction(this Component behaviour, float interval, Action onComplete, Action<float> onUpdate = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.PersistenceLoopAction(interval, onComplete, onUpdate, useRealTime, executeOnStart, behaviour);
    }
    
    public static LoopUntilTimer PersistenceLoopUntilAction(this Component behaviour, float interval, Func<LoopUntilTimer, bool> loopUntil, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.PersistenceLoopUntilAction(interval, loopUntil, onComplete, onUpdate, onFinished, useRealTime, executeOnStart, behaviour);
    }
    
    public static LoopCountTimer PersistenceLoopCountAction(this Component behaviour, float interval, int count, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.PersistenceLoopCountAction(interval, count, onComplete, onUpdate, onFinished, useRealTime, executeOnStart, behaviour);
    }
    
    public static void CancelAllTimer(this Component behaviour)
    {
        Timer.CancelAllRegisteredTimersByOwner(behaviour);
    }
}
