using System;
using GameUtil;
using UnityEngine;

/// <summary>
/// Contains extension methods related to <see cref="Timer"/>s.
/// </summary>
public static class TimerExtensions
{
    #region Component
    public static DelayTimer DelayAction(this Component component, float duration, Action onComplete, Action<float> onUpdate = null, bool useRealTime = false)
    {
        return Timer.DelayAction(duration, onComplete, onUpdate, useRealTime, component);
    }
    
    public static DelayFrameTimer DelayFrameAction(this Component component, int frame, Action onComplete, Action<float> onUpdate = null)
    {
        return Timer.DelayFrameAction(frame, onComplete, onUpdate, component);
    }
    
    public static LoopTimer LoopAction(this Component component, float interval, Action onComplete, Action<float> onUpdate = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.LoopAction(interval, onComplete, onUpdate, useRealTime, executeOnStart, component);
    }
    
    public static LoopUntilTimer LoopUntilAction(this Component component, float interval, Func<LoopUntilTimer, bool> loopUntil, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.LoopUntilAction(interval, loopUntil, onComplete, onUpdate, onFinished, useRealTime, executeOnStart, component);
    }
    
    public static LoopCountTimer LoopCountAction(this Component component, float interval, int count, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.LoopCountAction(interval, count, onComplete, onUpdate, onFinished, useRealTime, executeOnStart, component);
    }
    
    //Persistence
    public static DelayTimer PersistenceDelayAction(this Component component, float duration, Action onComplete, Action<float> onUpdate = null, bool useRealTime = false)
    {
        return Timer.PersistenceDelayAction(duration, onComplete, onUpdate, useRealTime, component);
    }
    
    public static DelayFrameTimer PersistenceDelayFrameAction(this Component component, int frame, Action onComplete, Action<float> onUpdate = null)
    {
        return Timer.PersistenceDelayFrameAction(frame, onComplete, onUpdate, component);
    }
    
    public static LoopTimer PersistenceLoopAction(this Component component, float interval, Action onComplete, Action<float> onUpdate = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.PersistenceLoopAction(interval, onComplete, onUpdate, useRealTime, executeOnStart, component);
    }
    
    public static LoopUntilTimer PersistenceLoopUntilAction(this Component component, float interval, Func<LoopUntilTimer, bool> loopUntil, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.PersistenceLoopUntilAction(interval, loopUntil, onComplete, onUpdate, onFinished, useRealTime, executeOnStart, component);
    }
    
    public static LoopCountTimer PersistenceLoopCountAction(this Component component, float interval, int count, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.PersistenceLoopCountAction(interval, count, onComplete, onUpdate, onFinished, useRealTime, executeOnStart, component);
    }
    
    public static void CancelAllTimer(this Component component)
    {
        Timer.CancelAllRegisteredTimersByOwner(component);
    }
    #endregion
    
    #region GameObject
    public static DelayTimer DelayAction(this GameObject gameObject, float duration, Action onComplete, Action<float> onUpdate = null, bool useRealTime = false)
    {
        return Timer.DelayAction(duration, onComplete, onUpdate, useRealTime, gameObject);
    }
    
    public static DelayFrameTimer DelayFrameAction(this GameObject component, int frame, Action onComplete, Action<float> onUpdate = null)
    {
        return Timer.DelayFrameAction(frame, onComplete, onUpdate, component);
    }
    
    public static LoopTimer LoopAction(this GameObject component, float interval, Action onComplete, Action<float> onUpdate = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.LoopAction(interval, onComplete, onUpdate, useRealTime, executeOnStart, component);
    }
    
    public static LoopUntilTimer LoopUntilAction(this GameObject component, float interval, Func<LoopUntilTimer, bool> loopUntil, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.LoopUntilAction(interval, loopUntil, onComplete, onUpdate, onFinished, useRealTime, executeOnStart, component);
    }
    
    public static LoopCountTimer LoopCountAction(this GameObject component, float interval, int count, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.LoopCountAction(interval, count, onComplete, onUpdate, onFinished, useRealTime, executeOnStart, component);
    }
    
    //Persistence
    public static DelayTimer PersistenceDelayAction(this GameObject component, float duration, Action onComplete, Action<float> onUpdate = null, bool useRealTime = false)
    {
        return Timer.PersistenceDelayAction(duration, onComplete, onUpdate, useRealTime, component);
    }
    
    public static DelayFrameTimer PersistenceDelayFrameAction(this GameObject component, int frame, Action onComplete, Action<float> onUpdate = null)
    {
        return Timer.PersistenceDelayFrameAction(frame, onComplete, onUpdate, component);
    }
    
    public static LoopTimer PersistenceLoopAction(this GameObject component, float interval, Action onComplete, Action<float> onUpdate = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.PersistenceLoopAction(interval, onComplete, onUpdate, useRealTime, executeOnStart, component);
    }
    
    public static LoopUntilTimer PersistenceLoopUntilAction(this GameObject component, float interval, Func<LoopUntilTimer, bool> loopUntil, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.PersistenceLoopUntilAction(interval, loopUntil, onComplete, onUpdate, onFinished, useRealTime, executeOnStart, component);
    }
    
    public static LoopCountTimer PersistenceLoopCountAction(this GameObject component, float interval, int count, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false)
    {
        return Timer.PersistenceLoopCountAction(interval, count, onComplete, onUpdate, onFinished, useRealTime, executeOnStart, component);
    }
    
    public static void CancelAllTimer(this GameObject component)
    {
        Timer.CancelAllRegisteredTimersByOwner(component);
    }
    #endregion
}
