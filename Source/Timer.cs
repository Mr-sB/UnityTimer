using UnityEngine;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace GameUtil
{
    /// <summary>
    /// Allows you to run events on a delay without the use of <see cref="Coroutine"/>s
    /// or <see cref="MonoBehaviour"/>s.
    /// </summary>
    public abstract class Timer
    {
        public enum UpdateMode
        {
            /// <summary>
            ///   <para>Update is based on Time.time. Time.timeScale does affect.</para>
            /// </summary>
            GameTime,
            /// <summary>
            ///   <para>Update is based on Time.unscaledTime. Time.timeScale does not affect, but Application pause does affect.</para>
            /// </summary>
            UnscaledGameTime,
            /// <summary>
            /// <para>Update is based on Time.realtimeSinceStartup. Time.timeScale and Application pause do not affect.</para>
            /// </summary>
            RealTime,
        }
    
        #region Public Properties/Fields

        /// <summary>
        /// How long the timer takes to complete from start to finish. (seconds/frame)
        /// </summary>
        public float duration { get; protected set; }

        /// <summary>
        /// whether the timer is persistence
        /// </summary>
        public bool isPersistence { get; }

        /// <summary>
        /// Whether or not the timer completed running. This is false if the timer was cancelled.
        /// </summary>
        public bool isCompleted { get; protected set; }

        /// <summary>
        /// Whether the timer uses real-time or game-time or unscaled-game-time. Real time is unaffected by changes to the timescale
        /// of the game(e.g. pausing, slow-mo), while game time is affected.
        /// </summary>
        public UpdateMode updateMode { get; protected set; }

        /// <summary>
        /// Whether the timer uses real-time or game-time. Real time is unaffected by changes to the timescale
        /// of the game(e.g. pausing, slow-mo), while game time is affected.
        /// </summary>
        [Obsolete("Use updateMode to instead.")]
        public bool usesRealTime => updateMode != UpdateMode.GameTime;

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

        // after the auto destroy owner is destroyed, the timer will expire
        // this way you don't run into any annoying bugs with timers running and accessing objects
        // after they have been destroyed
        public Object autoDestroyOwner { get; }
        public bool hasAutoDestroyOwner { get; }
        private bool isOwnerDestroyed
        {
            get
            {
                if (!hasAutoDestroyOwner || autoDestroyOwner) return false;
                if (!_timeElapsedBeforeAutoDestroy.HasValue)
                    _timeElapsedBeforeAutoDestroy = GetTimeElapsed();
                return true;
            }
        }
        
        #endregion

        #region Public Static Methods

        public static DelayTimer DelayAction(float duration, Action onComplete, Action<float> onUpdate = null,
            bool useRealTime = false, Object autoDestroyOwner = null)
        {
            return DelayActionInternal(false, duration, onComplete, onUpdate, useRealTime, autoDestroyOwner);
        }
        
        public static DelayTimer DelayAction(float duration, Action onComplete, Action<float> onUpdate,
            UpdateMode updateMode, Object autoDestroyOwner = null)
        {
            return DelayActionInternal(false, duration, onComplete, onUpdate, updateMode, autoDestroyOwner);
        }

        public static DelayFrameTimer DelayFrameAction(int frame, Action onComplete, Action<float> onUpdate = null,
            Object autoDestroyOwner = null)
        {
            return DelayFrameActionInternal(false, frame, onComplete, onUpdate, autoDestroyOwner);
        }

        public static LoopTimer LoopAction(float interval, Action<int> onComplete, Action<float> onUpdate = null,
            bool useRealTime = false, bool executeOnStart = false, Object autoDestroyOwner = null)
        {
            return LoopActionInternal(false, interval, onComplete, onUpdate, useRealTime, executeOnStart,
                autoDestroyOwner);
        }
        
        public static LoopTimer LoopAction(float interval, Action<int> onComplete, Action<float> onUpdate,
            UpdateMode updateMode, bool executeOnStart = false, Object autoDestroyOwner = null)
        {
            return LoopActionInternal(false, interval, onComplete, onUpdate, updateMode, executeOnStart,
                autoDestroyOwner);
        }
        
        public static LoopUntilTimer LoopUntilAction(float interval, Func<LoopUntilTimer, bool> loopUntil, Action<int> onComplete,
            Action<float> onUpdate = null, Action onFinished = null,
            bool useRealTime = false, bool executeOnStart = false, Object autoDestroyOwner = null)
        {
            return LoopUntilActionInternal(false, interval, loopUntil, onComplete, onUpdate, onFinished, useRealTime,
                executeOnStart, autoDestroyOwner);
        }
        
        public static LoopUntilTimer LoopUntilAction(float interval, Func<LoopUntilTimer, bool> loopUntil, Action<int> onComplete,
            Action<float> onUpdate, Action onFinished,
            UpdateMode updateMode, bool executeOnStart = false, Object autoDestroyOwner = null)
        {
            return LoopUntilActionInternal(false, interval, loopUntil, onComplete, onUpdate, onFinished, updateMode,
                executeOnStart, autoDestroyOwner);
        }

        public static LoopCountTimer LoopCountAction(float interval, int loopCount, Action<int> onComplete,
            Action<float> onUpdate = null, Action onFinished = null,
            bool useRealTime = false, bool executeOnStart = false, Object autoDestroyOwner = null)
        {
            return LoopCountActionInternal(false, interval, loopCount, onComplete, onUpdate, onFinished, useRealTime,
                executeOnStart, autoDestroyOwner);
        }
        
        public static LoopCountTimer LoopCountAction(float interval, int loopCount, Action<int> onComplete,
            Action<float> onUpdate, Action onFinished,
            UpdateMode updateMode, bool executeOnStart = false, Object autoDestroyOwner = null)
        {
            return LoopCountActionInternal(false, interval, loopCount, onComplete, onUpdate, onFinished, updateMode,
                executeOnStart, autoDestroyOwner);
        }

        //Persistence
        public static DelayTimer PersistenceDelayAction(float duration, Action onComplete,
            Action<float> onUpdate = null, bool useRealTime = false, Object autoDestroyOwner = null)
        {
            return DelayActionInternal(true, duration, onComplete, onUpdate, useRealTime, autoDestroyOwner);
        }
        
        public static DelayTimer PersistenceDelayAction(float duration, Action onComplete,
            Action<float> onUpdate, UpdateMode updateMode, Object autoDestroyOwner = null)
        {
            return DelayActionInternal(true, duration, onComplete, onUpdate, updateMode, autoDestroyOwner);
        }

        public static DelayFrameTimer PersistenceDelayFrameAction(int frame, Action onComplete,
            Action<float> onUpdate = null, Object autoDestroyOwner = null)
        {
            return DelayFrameActionInternal(true, frame, onComplete, onUpdate, autoDestroyOwner);
        }

        public static LoopTimer PersistenceLoopAction(float interval, Action<int> onComplete, Action<float> onUpdate = null,
            bool useRealTime = false, bool executeOnStart = false, Object autoDestroyOwner = null)
        {
            return LoopActionInternal(true, interval, onComplete, onUpdate, useRealTime, executeOnStart,
                autoDestroyOwner);
        }
        
        public static LoopTimer PersistenceLoopAction(float interval, Action<int> onComplete, Action<float> onUpdate,
            UpdateMode updateMode, bool executeOnStart = false, Object autoDestroyOwner = null)
        {
            return LoopActionInternal(true, interval, onComplete, onUpdate, updateMode, executeOnStart,
                autoDestroyOwner);
        }

        public static LoopUntilTimer PersistenceLoopUntilAction(float interval, Func<LoopUntilTimer, bool> loopUntil, Action<int> onComplete,
            Action<float> onUpdate = null, Action onFinished = null,
            bool useRealTime = false, bool executeOnStart = false, Object autoDestroyOwner = null)
        {
            return LoopUntilActionInternal(true, interval, loopUntil, onComplete, onUpdate, onFinished, useRealTime,
                executeOnStart, autoDestroyOwner);
        }
        
        public static LoopUntilTimer PersistenceLoopUntilAction(float interval, Func<LoopUntilTimer, bool> loopUntil, Action<int> onComplete,
            Action<float> onUpdate, Action onFinished,
            UpdateMode updateMode, bool executeOnStart = false, Object autoDestroyOwner = null)
        {
            return LoopUntilActionInternal(true, interval, loopUntil, onComplete, onUpdate, onFinished, updateMode,
                executeOnStart, autoDestroyOwner);
        }

        public static LoopCountTimer PersistenceLoopCountAction(float interval, int loopCount, Action<int> onComplete,
            Action<float> onUpdate = null, Action onFinished = null,
            bool useRealTime = false, bool executeOnStart = false, Object autoDestroyOwner = null)
        {
            return LoopCountActionInternal(true, interval, loopCount, onComplete, onUpdate, onFinished, useRealTime,
                executeOnStart, autoDestroyOwner);
        }
        
        public static LoopCountTimer PersistenceLoopCountAction(float interval, int loopCount, Action<int> onComplete,
            Action<float> onUpdate, Action onFinished,
            UpdateMode updateMode, bool executeOnStart = false, Object autoDestroyOwner = null)
        {
            return LoopCountActionInternal(true, interval, loopCount, onComplete, onUpdate, onFinished, updateMode,
                executeOnStart, autoDestroyOwner);
        }

        /// <summary>
        /// Restart a timer. The main benefit of this over the method on the instance is that you will not get
        /// a <see cref="NullReferenceException"/> if the timer is null.
        /// </summary>
        /// <param name="timer">The timer to restart.</param>
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

        public static void CancelAllRegisteredTimersByOwner(Object owner)
        {
            if (_manager != null)
            {
                _manager.CancelAllTimersByOwner(owner);
            }

            // if the manager doesn't exist, we don't have any registered timers yet, so don't
            // need to do anything in this case
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
            //auto destroy. return
            if (isOwnerDestroyed) return;

            isCompleted = false;
            _startTime = GetWorldTime();
            _lastUpdateTime = _startTime;
            _timeElapsedBeforeCancel = null;
            _timeElapsedBeforePause = null;
            _timeElapsedBeforeAutoDestroy = null;
            OnRestart();
            Register();
        }

        public void Restart(bool newUseRealTime)
        {
            Restart(GetUpdateMode(newUseRealTime));
        }
        
        public void Restart(UpdateMode newUpdateMode)
        {
            updateMode = newUpdateMode;
            Restart();
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
        /// Get how many seconds/frame have elapsed since the start of this timer's current cycle.
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
        /// Get how many seconds/frame remain before the timer completes.
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

        #region Private Static Methods

        private static void InitTimerManager()
        {
            if (_manager != null) return;
            // create a manager object to update all the timers if one does not already exist.
            _manager = Object.FindObjectOfType<TimerManager>();
            if (_manager == null)
                _manager = new GameObject(nameof(TimerManager)).AddComponent<TimerManager>();
            Object.DontDestroyOnLoad(_manager.transform.root.gameObject);
        }
        
        private static UpdateMode GetUpdateMode(bool usesRealTime)
        {
            return usesRealTime ? UpdateMode.RealTime : UpdateMode.GameTime;
        }

        private static DelayTimer DelayActionInternal(bool isPersistence, float duration, Action onComplete,
            Action<float> onUpdate, bool useRealTime, Object autoDestroyOwner)
        {
            //Check
            if (duration <= 0)
            {
                SafeCall(onUpdate, 0);
                SafeCall(onComplete);
                return null;
            }

            var timer = new DelayTimer(isPersistence, duration, onComplete, onUpdate, useRealTime, autoDestroyOwner);
            timer.Init();
            return timer;
        }
        
        private static DelayTimer DelayActionInternal(bool isPersistence, float duration, Action onComplete,
            Action<float> onUpdate, UpdateMode updateMode, Object autoDestroyOwner)
        {
            //Check
            if (duration <= 0)
            {
                SafeCall(onUpdate, 0);
                SafeCall(onComplete);
                return null;
            }

            var timer = new DelayTimer(isPersistence, duration, onComplete, onUpdate, updateMode, autoDestroyOwner);
            timer.Init();
            return timer;
        }

        private static DelayFrameTimer DelayFrameActionInternal(bool isPersistence, int frame, Action onComplete,
            Action<float> onUpdate, Object autoDestroyOwner)
        {
            //Check
            if (frame <= 0)
            {
                SafeCall(onUpdate, 0);
                SafeCall(onComplete);
                return null;
            }

            var timer = new DelayFrameTimer(isPersistence, frame, onComplete, onUpdate, autoDestroyOwner);
            timer.Init();
            return timer;
        }

        private static LoopTimer LoopActionInternal(bool isPersistence, float interval, Action<int> onComplete, Action<float> onUpdate,
            bool useRealTime, bool executeOnStart, Object autoDestroyOwner)
        {
            var timer = new LoopTimer(isPersistence, interval, onComplete, onUpdate, useRealTime, executeOnStart, autoDestroyOwner);
            timer.Init();
            return timer;
        }
        
        private static LoopTimer LoopActionInternal(bool isPersistence, float interval, Action<int> onComplete, Action<float> onUpdate,
            UpdateMode updateMode, bool executeOnStart, Object autoDestroyOwner)
        {
            var timer = new LoopTimer(isPersistence, interval, onComplete, onUpdate, updateMode, executeOnStart, autoDestroyOwner);
            timer.Init();
            return timer;
        }

        private static LoopUntilTimer LoopUntilActionInternal(bool isPersistence, float interval, Func<LoopUntilTimer, bool> loopUntil,
            Action<int> onComplete, Action<float> onUpdate, Action onFinished,
            bool useRealTime, bool executeOnStart, Object autoDestroyOwner)
        {
            var timer = new LoopUntilTimer(isPersistence, interval, loopUntil, onComplete, onUpdate,
                onFinished, useRealTime, executeOnStart, autoDestroyOwner);
            timer.Init();
            return timer;
        }
        
        private static LoopUntilTimer LoopUntilActionInternal(bool isPersistence, float interval, Func<LoopUntilTimer, bool> loopUntil,
            Action<int> onComplete, Action<float> onUpdate, Action onFinished,
            UpdateMode updateMode, bool executeOnStart, Object autoDestroyOwner)
        {
            var timer = new LoopUntilTimer(isPersistence, interval, loopUntil, onComplete, onUpdate,
                onFinished, updateMode, executeOnStart, autoDestroyOwner);
            timer.Init();
            return timer;
        }
        
        private static LoopCountTimer LoopCountActionInternal(bool isPersistence, float interval, int loopCount,
            Action<int> onComplete, Action<float> onUpdate, Action onFinished,
            bool useRealTime, bool executeOnStart, Object autoDestroyOwner)
        {
            //Check
            if (loopCount <= 0)
            {
                SafeCall(onUpdate, 0);
                SafeCall(onComplete, 1);
                SafeCall(onFinished);
                return null;
            }

            var timer = new LoopCountTimer(isPersistence, interval, loopCount, onComplete, onUpdate,
                onFinished, useRealTime, executeOnStart, autoDestroyOwner);
            timer.Init();
            return timer;
        }
        
        private static LoopCountTimer LoopCountActionInternal(bool isPersistence, float interval, int loopCount,
            Action<int> onComplete, Action<float> onUpdate, Action onFinished,
            UpdateMode updateMode, bool executeOnStart, Object autoDestroyOwner)
        {
            //Check
            if (loopCount <= 0)
            {
                SafeCall(onUpdate, 0);
                SafeCall(onComplete, 1);
                SafeCall(onFinished);
                return null;
            }

            var timer = new LoopCountTimer(isPersistence, interval, loopCount, onComplete, onUpdate,
                onFinished, updateMode, executeOnStart, autoDestroyOwner);
            timer.Init();
            return timer;
        }

        #endregion

        #region Private Static Properties/Fields

        // responsible for updating all registered timers
        private static TimerManager _manager;

        #endregion

        #region Private/Protected Properties/Fields
        

        // whether the timer is in TimeManager
        private bool _isInManager;

        protected Action<float> _onUpdate;
        protected float _startTime;
        private float _lastUpdateTime;

        // for pausing, we push the start time forward by the amount of time that has passed.
        // this will mess with the amount of time that elapsed when we're cancelled or paused if we just
        // check the start time versus the current world time, so we need to cache the time that was elapsed
        // before we paused/cancelled/autoDestroy
        private float? _timeElapsedBeforeCancel;
        private float? _timeElapsedBeforePause;
        private float? _timeElapsedBeforeAutoDestroy;

        private readonly LinkedListNode<Timer> _linkedListNode;

        #endregion

        #region Constructor (use static method to create new timer)

        static Timer()
        {
            InitTimerManager();
        }

        protected Timer(bool isPersistence, float duration, Action<float> onUpdate,
            bool usesRealTime, Object autoDestroyOwner)
            : this(isPersistence, duration, onUpdate, GetUpdateMode(usesRealTime), autoDestroyOwner)
        {
        }
        
        protected Timer(bool isPersistence, float duration, Action<float> onUpdate,
            UpdateMode updateMode, Object autoDestroyOwner)
        {
            this.isPersistence = isPersistence;
            this.duration = duration;
            this._onUpdate = onUpdate;

            this.updateMode = updateMode;

            this.autoDestroyOwner = autoDestroyOwner;
            this.hasAutoDestroyOwner = autoDestroyOwner != null;

            _linkedListNode = new LinkedListNode<Timer>(this);
        }

        #endregion

        #region Private/Protected Methods

        private void Init()
        {
            //avoid virtual member call in constructor
            _startTime = GetWorldTime();
            _lastUpdateTime = _startTime;
            Register();
            OnInit();
        }

        private void Register()
        {
            //no need to add
            if (_isInManager) return;
            _isInManager = true;
            _manager.Register(this);
        }

        protected float GetFireTime()
        {
            return _startTime + duration;
        }

        protected virtual float GetWorldTime()
        {
            switch (updateMode)
            {
                case UpdateMode.GameTime:
                    return Time.time;
                case UpdateMode.UnscaledGameTime:
                    return Time.unscaledTime;
                case UpdateMode.RealTime:
                    return Time.realtimeSinceStartup;
                default:
                    goto case UpdateMode.GameTime;
            }
        }

        protected virtual void OnInit()
        {
        }

        protected abstract void Update();

        protected virtual void OnRestart()
        {
        }

        protected bool CheckUpdate()
        {
            if (isDone) return false;

            if (isPaused)
            {
                var curTime = GetWorldTime();
                _startTime += curTime - _lastUpdateTime;
                _lastUpdateTime = curTime;
                return false;
            }

            _lastUpdateTime = GetWorldTime();
            return true;
        }

        protected static void SafeCall(Action action)
        {
            if (action == null) return;
            try
            {
                action();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        protected static void SafeCall<T>(Action<T> action, T arg)
        {
            if (action == null) return;
            try
            {
                action(arg);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        protected static TResult SafeCall<TResult>(Func<TResult> func)
        {
            if (func == null) return default;
            try
            {
                return func();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return default;
            }
        }
        
        protected static TResult SafeCall<T, TResult>(Func<T, TResult> func, T arg)
        {
            if (func == null) return default;
            try
            {
                return func(arg);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return default;
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
            private readonly LinkedList<Timer>
                _persistenceTimers =
                    new LinkedList<Timer>(); //can not be effected by Timer.xxAllRegisteredTimers() methods

            private readonly LinkedList<Timer> _timers = new LinkedList<Timer>();

            // buffer adding timers so we don't edit a collection during iteration
            private readonly List<Timer> _timersToAdd = new List<Timer>();
            private readonly List<Timer> _persistenceTimersToAdd = new List<Timer>();

            public void CancelAllTimers()
            {
                foreach (Timer timer in _timers)
                {
                    timer.Cancel();
                    timer._isInManager = false;
                }

                foreach (Timer timer in _timersToAdd)
                {
                    timer.Cancel();
                    timer._isInManager = false;
                }

                _timers.Clear();
                _timersToAdd.Clear();
            }

            public void CancelAllTimersByOwner(Object owner)
            {
                if (!owner) return;
                CancelAllTimersByOwner(_timers, _timersToAdd, owner);
                CancelAllTimersByOwner(_persistenceTimers, _persistenceTimersToAdd, owner);
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

            public void Register(Timer timer)
            {
                if (!timer.isPersistence)
                    _timersToAdd.Add(timer);
                else
                    _persistenceTimersToAdd.Add(timer);
            }

            // update all the registered timers on every frame
            private void Update()
            {
                UpdateTimers();
                UpdatePersistenceTimers();
            }

            //Timer
            private void UpdateTimers()
            {
                UpdateTimersInternal(_timers, _timersToAdd);
            }

            //PersistenceTimer
            private void UpdatePersistenceTimers()
            {
                UpdateTimersInternal(_persistenceTimers, _persistenceTimersToAdd);
            }

            private static void UpdateTimersInternal(LinkedList<Timer> timers, List<Timer> timersToAdd)
            {
                int toAddCount = timersToAdd.Count;
                if (toAddCount > 0)
                {
                    for (int i = 0; i < toAddCount; i++)
                        timers.AddLast(timersToAdd[i]._linkedListNode);
                    timersToAdd.Clear();
                }

                var node = timers.First;
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
                        timers.Remove(toRemoveNode);
                    }
                    else
                        node = node.Next;
                }
            }
            
            private static void CancelAllTimersByOwner(LinkedList<Timer> timers, List<Timer> timersToAdd, Object owner)
            {
                var node = timers.First;
                while (node != null)
                {
                    var timer = node.Value;
                    if (!timer.isDone && timer.autoDestroyOwner == owner)
                    {
                        timer.Cancel();
                        timer._isInManager = false;
                        var toRemoveNode = node;
                        node = node.Next;
                        //remove
                        timers.Remove(toRemoveNode);
                    }
                    else
                        node = node.Next;
                }

                for (int i = timersToAdd.Count - 1; i >= 0; i--)
                {
                    var timer = timersToAdd[i];
                    if (!timer.isDone && timer.autoDestroyOwner != owner) continue;
                    timer.Cancel();
                    timer._isInManager = false;
                    //remove
                    timersToAdd.RemoveAt(i);
                }
            }
        }

        #endregion

    }
}
