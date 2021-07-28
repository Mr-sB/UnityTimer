using System;
using UnityEngine;

namespace GameUtil
{
    public class LoopCountTimer : LoopUntilTimer
    {
        private int _loopCount;

        public LoopCountTimer(bool isPersistence, float interval, int loopCount, Action onComplete,
            Action<float> onUpdate, Action onFinished, bool usesRealTime, bool executeOnStart, MonoBehaviour autoDestroyOwner)
            : base(isPersistence, interval, null, onComplete, onUpdate, onFinished, usesRealTime, executeOnStart, autoDestroyOwner)
        {
            _loopCount = loopCount;
            _loopUntilFunc = LoopCountUntil;
        }

        private bool LoopCountUntil(LoopTimer timer)
        {
            return loopTime >= _loopCount;
        }

        public void Restart(float newInterval, int newLoopCount)
        {
            _loopCount = newLoopCount;
            base.Restart(newInterval);
        }
        
        public override void Restart(float newInterval, Func<LoopUntilTimer, bool> newLoopUntil)
        {
            base.Restart(newInterval, LoopCountUntil);
        }
        
        public override void Restart(float newInterval, Func<LoopUntilTimer, bool> newLoopUntil, Action newOnComplete, Action<float> newOnUpdate, Action newOnFinished, bool newUsesRealTime, bool newExecuteOnStart)
        {
            base.Restart(newInterval, LoopCountUntil, newOnComplete, newOnUpdate, newOnFinished, newUsesRealTime, newExecuteOnStart);
        }
        
        public void Restart(float newInterval, int newLoopCount, Action newOnComplete, Action<float> newOnUpdate, Action newOnFinished, bool newUsesRealTime, bool newExecuteOnStart)
        {
            _loopCount = newLoopCount;
            Restart(newInterval, LoopCountUntil, newOnComplete, newOnUpdate, newOnFinished, newUsesRealTime, newExecuteOnStart);
        }
    }
}
