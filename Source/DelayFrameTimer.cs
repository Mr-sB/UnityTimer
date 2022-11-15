using System;
using UnityEngine;

namespace GameUtil
{
    public class DelayFrameTimer : Timer
    {
        protected Action _onComplete;

        protected override float GetWorldTime()
        {
            return Time.frameCount;
        }

        public DelayFrameTimer(bool isPersistence, int frame, Action onComplete, Action<float> onUpdate,
            UnityEngine.Object autoDestroyOwner)
            : base(isPersistence, frame, onUpdate, UpdateMode.GameTime, autoDestroyOwner)
        {
            _onComplete = onComplete;
        }

        protected override void Update()
        {
            if (!CheckUpdate()) return;

            SafeCall(_onUpdate, GetTimeElapsed());
            //minus 1e-4 to avoid float precision cause equal judge fail
            if (GetWorldTime() >= GetFireTime() - 1e-4)
            {
                isCompleted = true;
                SafeCall(_onComplete);
            }
        }

        public void Restart(int newFrame)
        {
            duration = newFrame;
            Restart();
        }

        public void Restart(int newFrame, Action newOnComplete, Action<float> newOnUpdate)
        {
            duration = newFrame;
            _onComplete = newOnComplete;
            _onUpdate = newOnUpdate;
            Restart();
        }
    }
}