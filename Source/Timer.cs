/*
 * Unity Timer
 *
 * Version: 1.0
 * By: Alexander Biggs + Adam Robinson-Yu
 */

using UnityEngine;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

/// <summary>
/// Allows you to run events on a delay without the use of <see cref="Coroutine"/>s
/// or <see cref="MonoBehaviour"/>s.
///
/// To create and start a Timer, use the <see cref="Register"/> method.
/// </summary>
public class Timer
{
    #region Public Properties/Fields

    /// <summary>
    /// How long the timer takes to complete from start to finish.
    /// </summary>
    public float duration { get; private set; }

    /// <summary>
    /// Whether the timer will run again after completion.
    /// </summary>
    public bool isLooped { get; set; }

    /// <summary>
    /// Whether or not the timer completed running. This is false if the timer was cancelled.
    /// </summary>
    public bool isCompleted { get; private set; }

    /// <summary>
    /// Whether the timer uses real-time or game-time. Real time is unaffected by changes to the timescale
    /// of the game(e.g. pausing, slow-mo), while game time is affected.
    /// </summary>
    public bool usesRealTime { get; private set; }

    /// <summary>
    /// Whether the timer is currently paused.
    /// </summary>
    public bool isPaused
    {
        get { return this._timeElapsedBeforePause.HasValue; }
    }

    /// <summary>
    /// Whether or not the timer was cancelled.
    /// </summary>
    public bool isCancelled
    {
        get { return this._timeElapsedBeforeCancel.HasValue; }
    }

    /// <summary>
    /// Get whether or not the timer has finished running for any reason.
    /// </summary>
    public bool isDone
    {
        get { return this.isCompleted || this.isCancelled || this.isOwnerDestroyed; }
    }

    #endregion

    #region Public Static Methods

    public static Timer DelayAction(float duration, Action onComplete, Action<float> onUpdate = null, bool useRealTime = false, MonoBehaviour autoDestroyOwner = null)
    {
        if (onComplete == null && onUpdate == null) return null;
        return new Timer(false, duration, onComplete, onUpdate, false, useRealTime, autoDestroyOwner);
    }
    
    public static Timer DelayFrameAction(int frame, Action onComplete, Action<float> onUpdate = null, MonoBehaviour autoDestroyOwner = null)
    {
        if (onComplete == null && onUpdate == null) return null;

        int frameCount = 0;
        Timer timer = null;
        bool isFinished = false;
        void OnUpdateAction(float time)
        {
            onUpdate?.Invoke(time);
            frameCount++;
            if (frameCount >= frame)
            {
                Cancel(timer);
                if (!isFinished)
                {
                    onComplete?.Invoke();
                    isFinished = true;
                }
            }
        }
        
        timer = new Timer(false, 100000, onComplete, OnUpdateAction, false, true, autoDestroyOwner);
        return timer;
    }

    public static Timer LoopAction(float interval, Action onComplete, Action<float> onUpdate = null,
        bool useRealTime = false, bool executeOnStart = false, MonoBehaviour autoDestroyOwner = null)
    {
        if (onComplete == null && onUpdate == null) return null;
        if (executeOnStart && onComplete != null)
            onComplete();
        return new Timer(false, interval, onComplete, onUpdate, true, useRealTime, autoDestroyOwner);
    }
    
    public static Timer LoopAction(float interval, int count, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false, MonoBehaviour autoDestroyOwner = null)
    {
        if (onComplete == null && onUpdate == null && onFinished == null) return null;
        int timer = 0;
        Timer timerAction = null;
        bool isFinished = false;

        void OnCompleteAction()
        {
            if (timer >= count)
            {
                Cancel(timerAction);
                if (!isFinished)
                {
                    onFinished?.Invoke();
                    isFinished = true;
                }
                return;
            }

            onComplete?.Invoke();
            timer++;
            if (timer >= count)
            {
                Cancel(timerAction);
                if (!isFinished)
                {
                    onFinished?.Invoke();
                    isFinished = true;
                }
            }
        }

        if (executeOnStart)
            OnCompleteAction();
        timerAction = new Timer(false, interval, OnCompleteAction, onUpdate, true, useRealTime, autoDestroyOwner);
        return timerAction;
    }

    #region Persistence
    public static Timer PersistenceDelayAction(float duration, Action onComplete, Action<float> onUpdate = null, bool useRealTime = false, MonoBehaviour autoDestroyOwner = null)
    {
        if (onComplete == null && onUpdate == null) return null;
        return new Timer(true, duration, onComplete, onUpdate, false, useRealTime, autoDestroyOwner);
    }
    
    public static Timer PersistenceDelayFrameAction(int frame, Action onComplete, Action<float> onUpdate = null, MonoBehaviour autoDestroyOwner = null)
    {
        if (onComplete == null && onUpdate == null) return null;

        int frameCount = 0;
        Timer timer = null;
        bool isFinished = false;
        void OnUpdateAction(float time)
        {
            onUpdate?.Invoke(time);
            frameCount++;
            if (frameCount >= frame)
            {
                Cancel(timer);
                if (!isFinished)
                {
                    onComplete?.Invoke();
                    isFinished = true;
                }
            }
        }
        
        timer = new Timer(true, 100000, onComplete, OnUpdateAction, false, true, autoDestroyOwner);
        return timer;
    }

    public static Timer PersistenceLoopAction(float interval, Action onComplete, Action<float> onUpdate = null,
        bool useRealTime = false, bool executeOnStart = false, MonoBehaviour autoDestroyOwner = null)
    {
        if (onComplete == null && onUpdate == null) return null;
        if (executeOnStart && onComplete != null)
            onComplete();
        return new Timer(true, interval, onComplete, onUpdate, true, useRealTime, autoDestroyOwner);
    }
    
    public static Timer PersistenceLoopAction(float interval, int count, Action onComplete, Action<float> onUpdate = null, Action onFinished = null,
        bool useRealTime = false, bool executeOnStart = false, MonoBehaviour autoDestroyOwner = null)
    {
        if (onComplete == null && onUpdate == null && onFinished == null) return null;
        int timer = 0;
        Timer timerAction = null;
        bool isFinished = false;

        void OnCompleteAction()
        {
            if (timer >= count)
            {
                Cancel(timerAction);
                if (!isFinished)
                {
                    onFinished?.Invoke();
                    isFinished = true;
                }
                return;
            }

            onComplete?.Invoke();
            timer++;
            if (timer >= count)
            {
                Cancel(timerAction);
                if (!isFinished)
                {
                    onFinished?.Invoke();
                    isFinished = true;
                }
            }
        }

        if (executeOnStart)
            OnCompleteAction();
        timerAction = new Timer(true, interval, OnCompleteAction, onUpdate, true, useRealTime, autoDestroyOwner);
        return timerAction;
    }
    #endregion

    /// <summary>
    /// Restart a timer. The main benefit of this over the method on the instance is that you will not get
    /// a <see cref="NullReferenceException"/> if the timer is null.
    /// </summary>
    /// <param name="timer">The timer to cancel.</param>
    public static void Restart(Timer timer)
    {
        if (timer != null)
        {
            timer.Restart();
        }
    }
    
    /// <summary>
    /// Cancels a timer. The main benefit of this over the method on the instance is that you will not get
    /// a <see cref="NullReferenceException"/> if the timer is null.
    /// </summary>
    /// <param name="timer">The timer to cancel.</param>
    public static void Cancel(Timer timer)
    {
        if (timer != null)
        {
            timer.Cancel();
        }
    }

    /// <summary>
    /// Pause a timer. The main benefit of this over the method on the instance is that you will not get
    /// a <see cref="NullReferenceException"/> if the timer is null.
    /// </summary>
    /// <param name="timer">The timer to pause.</param>
    public static void Pause(Timer timer)
    {
        if (timer != null)
        {
            timer.Pause();
        }
    }

    /// <summary>
    /// Resume a timer. The main benefit of this over the method on the instance is that you will not get
    /// a <see cref="NullReferenceException"/> if the timer is null.
    /// </summary>
    /// <param name="timer">The timer to resume.</param>
    public static void Resume(Timer timer)
    {
        if (timer != null)
        {
            timer.Resume();
        }
    }

    public static void CancelAllRegisteredTimers()
    {
        if (_manager != null)
        {
            _manager.CancelAllTimers();
        }

        // if the manager doesn't exist, we don't have any registered timers yet, so don't
        // need to do anything in this case
    }

    public static void PauseAllRegisteredTimers()
    {
        if (_manager != null)
        {
            _manager.PauseAllTimers();
        }

        // if the manager doesn't exist, we don't have any registered timers yet, so don't
        // need to do anything in this case
    }

    public static void ResumeAllRegisteredTimers()
    {
        if (_manager != null)
        {
            _manager.ResumeAllTimers();
        }

        // if the manager doesn't exist, we don't have any registered timers yet, so don't
        // need to do anything in this case
    }

    #endregion

    #region Public Methods
    
    /// <summary>
    /// Restart a timer that is in-progress or done. The timer's on completion callback will not be called.
    /// </summary>
    public void Restart()
    {
        isCompleted = false;
        _startTime = GetWorldTime();
        _lastUpdateTime = _startTime;
        _timeElapsedBeforeCancel = null;
        _timeElapsedBeforePause = null;
        _timeElapsedBeforeAutoDestroy = null;
        _manager.Register(this);
    }
    
    /// <summary>
    /// Stop a timer that is in-progress or paused. The timer's on completion callback will not be called.
    /// </summary>
    public void Cancel()
    {
        if (this.isDone)
        {
            return;
        }

        this._timeElapsedBeforeCancel = this.GetTimeElapsed();
        this._timeElapsedBeforePause = null;
    }

    /// <summary>
    /// Pause a running timer. A paused timer can be resumed from the same point it was paused.
    /// </summary>
    public void Pause()
    {
        if (this.isPaused || this.isDone)
        {
            return;
        }

        this._timeElapsedBeforePause = this.GetTimeElapsed();
    }

    /// <summary>
    /// Continue a paused timer. Does nothing if the timer has not been paused.
    /// </summary>
    public void Resume()
    {
        if (!this.isPaused || this.isDone)
        {
            return;
        }

        this._timeElapsedBeforePause = null;
    }

    /// <summary>
    /// Get how many seconds have elapsed since the start of this timer's current cycle.
    /// </summary>
    /// <returns>The number of seconds that have elapsed since the start of this timer's current cycle, i.e.
    /// the current loop if the timer is looped, or the start if it isn't.
    ///
    /// If the timer has finished running, this is equal to the duration.
    ///
    /// If the timer was cancelled/paused, this is equal to the number of seconds that passed between the timer
    /// starting and when it was cancelled/paused.</returns>
    public float GetTimeElapsed()
    {
        if (this.isCompleted)
        {
            return this.duration;
        }

        return this._timeElapsedBeforeCancel ??
               this._timeElapsedBeforePause ??
               this._timeElapsedBeforeAutoDestroy ??
               this.GetWorldTime() - this._startTime;
    }

    /// <summary>
    /// Get how many seconds remain before the timer completes.
    /// </summary>
    /// <returns>The number of seconds that remain to be elapsed until the timer is completed. A timer
    /// is only elapsing time if it is not paused, cancelled, or completed. This will be equal to zero
    /// if the timer completed.</returns>
    public float GetTimeRemaining()
    {
        return this.duration - this.GetTimeElapsed();
    }

    /// <summary>
    /// Get how much progress the timer has made from start to finish as a ratio.
    /// </summary>
    /// <returns>A value from 0 to 1 indicating how much of the timer's duration has been elapsed.</returns>
    public float GetRatioComplete()
    {
        return this.GetTimeElapsed() / this.duration;
    }

    /// <summary>
    /// Get how much progress the timer has left to make as a ratio.
    /// </summary>
    /// <returns>A value from 0 to 1 indicating how much of the timer's duration remains to be elapsed.</returns>
    public float GetRatioRemaining()
    {
        return this.GetTimeRemaining() / this.duration;
    }

    #endregion

    #region Private Static Properties/Fields

    // responsible for updating all registered timers
    private static TimerManager _manager;

    #endregion

    #region Private Properties/Fields

    private bool isOwnerDestroyed
    {
        get
        {
            if (!_hasAutoDestroyOwner || _autoDestroyOwner) return false;
            if (!_timeElapsedBeforeAutoDestroy.HasValue)
                _timeElapsedBeforeAutoDestroy = GetTimeElapsed();
            return true;
        }
    }

    // whether the timer is persistence
    private readonly bool _isPersistence;
    // whether the timer is in TimeManager
    private bool _isInManager;
    
    private readonly Action _onComplete;
    private readonly Action<float> _onUpdate;
    private float _startTime;
    private float _lastUpdateTime;

    // for pausing, we push the start time forward by the amount of time that has passed.
    // this will mess with the amount of time that elapsed when we're cancelled or paused if we just
    // check the start time versus the current world time, so we need to cache the time that was elapsed
    // before we paused/cancelled/autoDestroy
    private float? _timeElapsedBeforeCancel;
    private float? _timeElapsedBeforePause;
    private float? _timeElapsedBeforeAutoDestroy;

    // after the auto destroy owner is destroyed, the timer will expire
    // this way you don't run into any annoying bugs with timers running and accessing objects
    // after they have been destroyed
    private readonly MonoBehaviour _autoDestroyOwner;
    private readonly bool _hasAutoDestroyOwner;

    private readonly LinkedListNode<Timer> _linkedListNode;

    #endregion

    #region Private Constructor (use static Register method to create new timer)

    static Timer()
    {
        // create a manager object to update all the timers if one does not already exist.
        if (_manager == null)
        {
            Init();
        }
    }
    
    private Timer(bool isPersistence, float duration, Action onComplete, Action<float> onUpdate,
        bool isLooped, bool usesRealTime, MonoBehaviour autoDestroyOwner)
    {
        this._isPersistence = isPersistence;
        this.duration = duration;
        this._onComplete = onComplete;
        this._onUpdate = onUpdate;

        this.isLooped = isLooped;
        this.usesRealTime = usesRealTime;

        this._autoDestroyOwner = autoDestroyOwner;
        this._hasAutoDestroyOwner = autoDestroyOwner != null;

        this._startTime = this.GetWorldTime();
        this._lastUpdateTime = this._startTime;
        
        _linkedListNode = new LinkedListNode<Timer>(this);
        
        _manager.Register(this);
    }

    #endregion

    #region Private Methods

    private static void Init()
    {
        // create a manager object to update all the timers if one does not already exist.
        if (_manager == null)
        {
            TimerManager managerInScene = Object.FindObjectOfType<TimerManager>();
            if (managerInScene != null)
            {
                _manager = managerInScene;
            }
            else
            {
                GameObject managerObject = new GameObject("TimerManager");
                _manager = managerObject.AddComponent<TimerManager>();
            }
            Object.DontDestroyOnLoad(_manager.gameObject.transform.root.gameObject);
        }
    }
    
    private float GetWorldTime()
    {
        return this.usesRealTime ? Time.realtimeSinceStartup : Time.time;
    }

    private float GetFireTime()
    {
        return this._startTime + this.duration;
    }

    private float GetTimeDelta()
    {
        return this.GetWorldTime() - this._lastUpdateTime;
    }

    private void Update()
    {
        if (this.isDone)
        {
            return;
        }

        if (this.isPaused)
        {
            this._startTime += this.GetTimeDelta();
            this._lastUpdateTime = this.GetWorldTime();
            return;
        }

        this._lastUpdateTime = this.GetWorldTime();

        if (this._onUpdate != null)
        {
            try
            {
                this._onUpdate(this.GetTimeElapsed());
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        if (this.GetWorldTime() >= this.GetFireTime())
        {
            if (this._onComplete != null)
            {
                try
                {
                    this._onComplete();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            
            if (this.isLooped)
            {
                this._startTime = this.GetWorldTime();
            }
            else
            {
                this.isCompleted = true;
            }
        }
    }

    #endregion

    #region Manager Class (implementation detail, spawned automatically and updates all registered timers)

    /// <summary>
    /// Manages updating all the <see cref="Timer"/>s that are running in the application.
    /// This will be instantiated the first time you create a timer -- you do not need to add it into the
    /// scene manually.
    /// </summary>
    private class TimerManager : MonoBehaviour
    {
        //不会被Timer.xxAll的方法影响
        private readonly LinkedList<Timer> _persistenceTimers = new LinkedList<Timer>();
        private readonly LinkedList<Timer> _timers = new LinkedList<Timer>();

        // buffer adding timers so we don't edit a collection during iteration
        private readonly List<Timer> _timersToAdd = new List<Timer>();
        private readonly List<Timer> _persistenceTimersToAdd = new List<Timer>();
        
        public void CancelAllTimers()
        {
            foreach (Timer timer in _timers)
            {
                timer.Cancel();
            }
            
            foreach (Timer timer in _timersToAdd)
            {
                timer.Cancel();
            }

            _timers.Clear();
            _timersToAdd.Clear();
        }

        public void PauseAllTimers()
        {
            foreach (Timer timer in _timers)
            {
                timer.Pause();
            }
            
            foreach (Timer timer in _timersToAdd)
            {
                timer.Pause();
            }
        }

        public void ResumeAllTimers()
        {
            foreach (Timer timer in _timers)
            {
                timer.Resume();
            }
            
            foreach (Timer timer in _timersToAdd)
            {
                timer.Resume();
            }
        }

        // update all the registered timers on every frame
        private void Update()
        {
            UpdateTimers();
            UpdatePersistenceTimers();
        }
        
        //Timer
        public void Register(Timer timer)
        {
            //no need to add
            if (timer._isInManager) return;
            timer._isInManager = true;
            if(!timer._isPersistence)
                _timersToAdd.Add(timer);
            else
                _persistenceTimersToAdd.Add(timer);
        }
        
        private void UpdateTimers()
        {
            int toAddCount = _timersToAdd.Count;
            if (toAddCount > 0)
            {
                for (int i = 0; i < toAddCount; i++)
                    _timers.AddLast(_timersToAdd[i]._linkedListNode);
                _timersToAdd.Clear();
            }

            var node = _timers.First;
            while (node != null)
            {
                var timer = node.Value;
                timer.Update();
                if (timer.isDone)
                {
                    timer._isInManager = false;
                    var toRemoveNode = node;
                    node = node.Next;
                    //remove
                    _timers.Remove(toRemoveNode);
                }
                else
                    node = node.Next;
            }
        }
        
        //PersistenceTimer
        private void UpdatePersistenceTimers()
        {
            int toAddCount = _persistenceTimersToAdd.Count;
            if (toAddCount > 0)
            {
                for (int i = 0; i < toAddCount; i++)
                    _persistenceTimers.AddLast(_persistenceTimersToAdd[i]._linkedListNode);
                _persistenceTimersToAdd.Clear();
            }

            var node = _persistenceTimers.First;
            while (node != null)
            {
                var timer = node.Value;
                timer.Update();
                if (timer.isDone)
                {
                    timer._isInManager = false;
                    var toRemoveNode = node;
                    node = node.Next;
                    //remove
                    _persistenceTimers.Remove(toRemoveNode);
                }
                else
                    node = node.Next;
            }
        }
    }

    #endregion

}
