using System;
using UnityEngine;

/// <summary>
/// Contains extension methods related to <see cref="Timer"/>s.
/// </summary>
public static class TimerExtensions
{
    public static Timer DelayAction(this MonoBehaviour behaviour, float duration, Action onComplete, Action<float> onUpdate = null, bool useRealTime = false)
    {
        return Timer.DelayAction(duration, onComplete, onUpdate, useRealTime, behaviour);
    }
    
    public static Timer DelayFrameAction(this MonoBehaviour behaviour, int frame, Action onComplete, Action<float> onUpdate = null)
    {
        return Timer.DelayFrameAction(frame, onComplete, onUpdate, behaviour);
    }
    
    public static Timer LoopAction(this MonoBehaviour behaviour, float interval, Action onComplete, Action<float> onUpdate = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.LoopAction(interval, onComplete, onUpdate, useRealTime, executeOnStart, behaviour);
    }
    
    public static Timer LoopAction(this MonoBehaviour behaviour, float interval, int count, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.LoopAction(interval, count, onComplete, onUpdate, onFinished, useRealTime, executeOnStart, behaviour);
    }
    
    //Persistence
    public static Timer PersistenceDelayAction(this MonoBehaviour behaviour, float duration, Action onComplete, Action<float> onUpdate = null, bool useRealTime = false)
    {
        return Timer.PersistenceDelayAction(duration, onComplete, onUpdate, useRealTime, behaviour);
    }
    
    public static Timer PersistenceDelayFrameAction(this MonoBehaviour behaviour, int frame, Action onComplete, Action<float> onUpdate = null)
    {
        return Timer.PersistenceDelayFrameAction(frame, onComplete, onUpdate, behaviour);
    }
    
    public static Timer PersistenceLoopAction(this MonoBehaviour behaviour, float interval, Action onComplete, Action<float> onUpdate = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.PersistenceLoopAction(interval, onComplete, onUpdate, useRealTime, executeOnStart, behaviour);
    }
    
    public static Timer PersistenceLoopAction(this MonoBehaviour behaviour, float interval, int count, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.PersistenceLoopAction(interval, count, onComplete, onUpdate, onFinished, useRealTime, executeOnStart, behaviour);
    }
}
