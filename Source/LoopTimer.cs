using System;

namespace GameUtil
{
    public class LoopTimer : Timer
    {
        private bool _executeOnStart;

        /// <summary>
        /// How many times does the LoopTimer looped.
        /// </summary>
        public int loopTime { protected set; get; }

        protected virtual void OnComplete()
        {
        }

        public LoopTimer(bool isPersistence, float interval, Action onComplete,
            Action<float> onUpdate, bool usesRealTime, bool executeOnStart, UnityEngine.Object autoDestroyOwner)
            : base(isPersistence, interval, onComplete, onUpdate, usesRealTime, autoDestroyOwner)
        {
            _executeOnStart = executeOnStart;
        }

        protected override void OnInit()
        {
            //avoid virtual member call in constructor
            if (_executeOnStart)
                Complete();
        }

        protected override void OnRestart()
        {
            loopTime = 0;
            if (_executeOnStart)
                Complete();
        }

        protected override void Update()
        {
            if (!CheckUpdate()) return;

            if (_onUpdate != null)
                SafeCall(_onUpdate, GetTimeElapsed());

            var timeDifference = GetWorldTime() - GetFireTime();
            //Loop call until cannot fire
            while (timeDifference >= 0)
            {
                Complete();
                if (isCompleted)
                    break;
                _startTime = GetWorldTime() - timeDifference; //Avoid time error accumulation
                timeDifference = GetWorldTime() - GetFireTime();
            }
        }

        private void Complete()
        {
            loopTime++;
            SafeCall(_onComplete);
            OnComplete();
        }

        public void Restart(float newInterval)
        {
            duration = newInterval;
            Restart();
        }

        public void Restart(float newInterval, Action newOnComplete, Action<float> newOnUpdate, bool newUsesRealTime, bool newExecuteOnStart)
        {
            duration = newInterval;
            _onComplete = newOnComplete;
            _onUpdate = newOnUpdate;
            usesRealTime = newUsesRealTime;
            _executeOnStart = newExecuteOnStart;
            Restart();
        }
    }
}