using System;
using GameUtil;
using UnityEngine;

/// <summary>
/// Contains extension methods related to <see cref="Timer"/>s.
/// </summary>
public static class TimerExtensions
{
    public static DelayTimer DelayAction(this MonoBehaviour behaviour, float duration, Action onComplete, Action<float> onUpdate = null, bool useRealTime = false)
    {
        return Timer.DelayAction(duration, onComplete, onUpdate, useRealTime, behaviour);
    }
    
    public static DelayFrameTimer DelayFrameAction(this MonoBehaviour behaviour, int frame, Action onComplete, Action<float> onUpdate = null)
    {
        return Timer.DelayFrameAction(frame, onComplete, onUpdate, behaviour);
    }
    
    public static LoopTimer LoopAction(this MonoBehaviour behaviour, float interval, Action onComplete, Action<float> onUpdate = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.LoopAction(interval, onComplete, onUpdate, useRealTime, executeOnStart, behaviour);
    }
    
    public static LoopUntilTimer LoopUntilAction(this MonoBehaviour behaviour, float interval, Func<LoopUntilTimer, bool> loopUntil, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.LoopUntilAction(interval, loopUntil, onComplete, onUpdate, onFinished, useRealTime, executeOnStart, behaviour);
    }
    
    public static LoopCountTimer LoopCountAction(this MonoBehaviour behaviour, float interval, int count, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.LoopCountAction(interval, count, onComplete, onUpdate, onFinished, useRealTime, executeOnStart, behaviour);
    }
    
    //Persistence
    public static DelayTimer PersistenceDelayAction(this MonoBehaviour behaviour, float duration, Action onComplete, Action<float> onUpdate = null, bool useRealTime = false)
    {
        return Timer.PersistenceDelayAction(duration, onComplete, onUpdate, useRealTime, behaviour);
    }
    
    public static DelayFrameTimer PersistenceDelayFrameAction(this MonoBehaviour behaviour, int frame, Action onComplete, Action<float> onUpdate = null)
    {
        return Timer.PersistenceDelayFrameAction(frame, onComplete, onUpdate, behaviour);
    }
    
    public static LoopTimer PersistenceLoopAction(this MonoBehaviour behaviour, float interval, Action onComplete, Action<float> onUpdate = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.PersistenceLoopAction(interval, onComplete, onUpdate, useRealTime, executeOnStart, behaviour);
    }
    
    public static LoopUntilTimer PersistenceLoopUntilAction(this MonoBehaviour behaviour, float interval, Func<LoopUntilTimer, bool> loopUntil, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.PersistenceLoopUntilAction(interval, loopUntil, onComplete, onUpdate, onFinished, useRealTime, executeOnStart, behaviour);
    }
    
    public static LoopCountTimer PersistenceLoopCountAction(this MonoBehaviour behaviour, float interval, int count, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.PersistenceLoopCountAction(interval, count, onComplete, onUpdate, onFinished, useRealTime, executeOnStart, behaviour);
    }
}
