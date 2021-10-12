using System;

namespace GameUtil
{
    public class DelayTimer : Timer
    {
        public DelayTimer(bool isPersistence, float duration, Action onComplete, Action<float> onUpdate,
            bool usesRealTime, UnityEngine.Object autoDestroyOwner)
            : base(isPersistence, duration, onComplete, onUpdate, usesRealTime, autoDestroyOwner)
        {
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

        public void Restart(bool newUseRealTime)
        {
            usesRealTime = newUseRealTime;
            Restart();
        }

        public void Restart(float newDuration, bool newUseRealTime)
        {
            duration = newDuration;
            usesRealTime = newUseRealTime;
            Restart();
        }

        public void Restart(float newDuration, Action newOnComplete, Action<float> newOnUpdate, bool newUseRealTime)
        {
            duration = newDuration;
            usesRealTime = newUseRealTime;
            _onComplete = newOnComplete;
            _onUpdate = newOnUpdate;
            Restart();
        }
    }
}