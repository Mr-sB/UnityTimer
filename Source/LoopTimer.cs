using System;

namespace GameUtil
{
    public class LoopTimer : Timer
    {
        /// <summary>
        /// Parameter is loopTime.
        /// </summary>
        protected Action<int> _onComplete;
        public bool executeOnStart { protected set; get; }

        [Obsolete("Use loopTimes instead.")]
        public int loopTime => loopTimes;
        /// <summary>
        /// How many times does the LoopTimer looped.
        /// </summary>
        public int loopTimes { private set; get; }

        protected virtual void OnComplete()
        {
        }

        public LoopTimer(bool isPersistence, float interval, Action<int> onComplete,
            Action<float> onUpdate, bool usesRealTime, bool executeOnStart, UnityEngine.Object autoDestroyOwner)
            : base(isPersistence, interval, onUpdate, usesRealTime, autoDestroyOwner)
        {
            _onComplete = onComplete;
            this.executeOnStart = executeOnStart;
        }
        
        public LoopTimer(bool isPersistence, float interval, Action<int> onComplete,
            Action<float> onUpdate, UpdateMode updateMode, bool executeOnStart, UnityEngine.Object autoDestroyOwner)
            : base(isPersistence, interval, onUpdate, updateMode, autoDestroyOwner)
        {
            _onComplete = onComplete;
            this.executeOnStart = executeOnStart;
        }

        protected override void OnInit()
        {
            //avoid virtual member call in constructor
            if (executeOnStart)
                Complete();
        }

        protected override void OnRestart()
        {
            loopTimes = 0;
            if (executeOnStart)
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
                if (isDone)
                    break;
                _startTime = GetWorldTime() - timeDifference; //Avoid time error accumulation
                timeDifference = GetWorldTime() - GetFireTime();
            }
        }

        private void Complete()
        {
            loopTimes++;
            SafeCall(_onComplete, loopTimes);
            OnComplete();
        }

        public void Restart(float newInterval)
        {
            duration = newInterval;
            Restart();
        }

        public void Restart(float newInterval, Action<int> newOnComplete, Action<float> newOnUpdate, bool newUsesRealTime, bool newExecuteOnStart)
        {
            duration = newInterval;
            _onComplete = newOnComplete;
            _onUpdate = newOnUpdate;
            executeOnStart = newExecuteOnStart;
            Restart(newUsesRealTime);
        }
        
        public void Restart(float newInterval, Action<int> newOnComplete, Action<float> newOnUpdate, UpdateMode newUpdateMode, bool newExecuteOnStart)
        {
            duration = newInterval;
            _onComplete = newOnComplete;
            _onUpdate = newOnUpdate;
            executeOnStart = newExecuteOnStart;
            Restart(newUpdateMode);
        }
    }
}