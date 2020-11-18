using System;
using UnityEngine;

namespace GameUtil
{
    public class LoopTimer : Timer
    {
        private bool _executeOnStart;
        private int? _loopCount;
        private Action _onFinished;
        private int _timer;

        public LoopTimer(bool isPersistence, float interval, Action onComplete,
            Action<float> onUpdate, bool usesRealTime, bool executeOnStart, MonoBehaviour autoDestroyOwner)
            : base(isPersistence, interval, onComplete, onUpdate, usesRealTime, autoDestroyOwner)
        {
            _executeOnStart = executeOnStart;
            if (_executeOnStart)
                OnComplete();
        }

        public LoopTimer(bool isPersistence, float interval, int loopCount, Action onComplete,
            Action<float> onUpdate, Action onFinished, bool usesRealTime, bool executeOnStart, MonoBehaviour autoDestroyOwner)
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
            if (!CheckUpdate()) return;

            if (_onUpdate != null)
                SafeCall(_onUpdate, GetTimeElapsed());

            var timeDifference = GetWorldTime() - GetFireTime();
            if (timeDifference >= 0)
            {
                OnComplete();
                if (!isCompleted)
                    _startTime = GetWorldTime() - timeDifference; //Avoid time error accumulation
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

        public void Restart(float newInterval, Action newOnComplete, Action<float> newOnUpdate, bool newUsesRealTime, bool newExecuteOnStart)
        {
            duration = newInterval;
            _loopCount = null;
            _onComplete = newOnComplete;
            _onUpdate = newOnUpdate;
            _onFinished = null;
            usesRealTime = newUsesRealTime;
            _executeOnStart = newExecuteOnStart;
            Restart();
        }
        
        public void Restart(float newInterval, int newLoopCount, Action newOnComplete, Action<float> newOnUpdate, Action newOnFinished, bool newUsesRealTime, bool newExecuteOnStart)
        {
            duration = newInterval;
            _loopCount = newLoopCount;
            _onComplete = newOnComplete;
            _onUpdate = newOnUpdate;
            _onFinished = newOnFinished;
            usesRealTime = newUsesRealTime;
            _executeOnStart = newExecuteOnStart;
            Restart();
        }
    }
}