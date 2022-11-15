using System;

namespace GameUtil
{
    public class DelayTimer : Timer
    {
        protected Action _onComplete;

        public DelayTimer(bool isPersistence, float duration, Action onComplete, Action<float> onUpdate,
            bool usesRealTime, UnityEngine.Object autoDestroyOwner)
            : base(isPersistence, duration, onUpdate, usesRealTime, autoDestroyOwner)
        {
            _onComplete = onComplete;
        }
        
        public DelayTimer(bool isPersistence, float duration, Action onComplete, Action<float> onUpdate,
            UpdateMode updateMode, UnityEngine.Object autoDestroyOwner)
            : base(isPersistence, duration, onUpdate, updateMode, autoDestroyOwner)
        {
            _onComplete = onComplete;
        }

        protected override void Update()
        {
            if (!CheckUpdate()) return;

            if (_onUpdate != null)
                SafeCall(_onUpdate, GetTimeElapsed());

            if (GetWorldTime() >= GetFireTime())
            {
                isCompleted = true;
                SafeCall(_onComplete);
            }
        }

        public void Restart(float newDuration)
        {
            duration = newDuration;
            Restart();
        }

        public void Restart(float newDuration, bool newUseRealTime)
        {
            duration = newDuration;
            Restart(newUseRealTime);
        }
        
        public void Restart(float newDuration, UpdateMode newUpdateMode)
        {
            duration = newDuration;
            Restart(newUpdateMode);
        }

        public void Restart(float newDuration, Action newOnComplete, Action<float> newOnUpdate, bool newUseRealTime)
        {
            duration = newDuration;
            _onComplete = newOnComplete;
            _onUpdate = newOnUpdate;
            Restart(newUseRealTime);
        }
        
        public void Restart(float newDuration, Action newOnComplete, Action<float> newOnUpdate, UpdateMode newUpdateMode)
        {
            duration = newDuration;
            _onComplete = newOnComplete;
            _onUpdate = newOnUpdate;
            Restart(newUpdateMode);
        }
    }
}