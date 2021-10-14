using System;

namespace GameUtil
{
    public class LoopCountTimer : LoopUntilTimer
    {
        private int _loopCount;

        public LoopCountTimer(bool isPersistence, float interval, int loopCount, Action<int> onComplete,
            Action<float> onUpdate, Action onFinished, bool usesRealTime, bool executeOnStart, UnityEngine.Object autoDestroyOwner)
            : base(isPersistence, interval, null, onComplete, onUpdate, onFinished, usesRealTime, executeOnStart, autoDestroyOwner)
        {
            _loopCount = loopCount;
            _loopUntilFunc = LoopCountUntil;
        }

        private bool LoopCountUntil(LoopUntilTimer timer)
        {
            return loopTimes >= _loopCount;
        }

        public void Restart(float newInterval, int newLoopCount)
        {
            _loopCount = newLoopCount;
            base.Restart(newInterval);
        }
        
        //Avoid _loopUntilFunc reassignment
        public sealed override void Restart(float newInterval, Func<LoopUntilTimer, bool> newLoopUntil)
        {
            base.Restart(newInterval, LoopCountUntil);
        }
        
        //Avoid _loopUntilFunc reassignment
        public sealed override void Restart(float newInterval, Func<LoopUntilTimer, bool> newLoopUntil, Action<int> newOnComplete, Action<float> newOnUpdate, Action newOnFinished, bool newUsesRealTime, bool newExecuteOnStart)
        {
            base.Restart(newInterval, LoopCountUntil, newOnComplete, newOnUpdate, newOnFinished, newUsesRealTime, newExecuteOnStart);
        }
        
        public void Restart(float newInterval, int newLoopCount, Action<int> newOnComplete, Action<float> newOnUpdate, Action newOnFinished, bool newUsesRealTime, bool newExecuteOnStart)
        {
            _loopCount = newLoopCount;
            Restart(newInterval, LoopCountUntil, newOnComplete, newOnUpdate, newOnFinished, newUsesRealTime, newExecuteOnStart);
        }
    }
}
