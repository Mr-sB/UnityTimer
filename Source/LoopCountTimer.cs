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
        }

        protected override void OnInit()
        {
            //Cannot access non-static method "LoopCountUntil" in static context ".Ctor"
            _loopUntilFunc = LoopCountUntil;
            base.OnInit();
        }

        private bool LoopCountUntil(LoopTimer timer)
        {
            return loopTime >= _loopCount;
        }

        public override void Restart(float newInterval, Func<LoopUntilTimer, bool> newLoopUntil, Action newOnComplete, Action<float> newOnUpdate, Action newOnFinished, bool newUsesRealTime, bool newExecuteOnStart)
        {
            base.Restart(newInterval, LoopCountUntil, newOnComplete, newOnUpdate, newOnFinished, newUsesRealTime, newExecuteOnStart);
        }
        
        public void Restart(float newInterval, int newLoopCount, Action newOnComplete, Action<float> newOnUpdate, Action newOnFinished, bool newUsesRealTime, bool newExecuteOnStart)
        {
            _loopCount = newLoopCount;
            base.Restart(newInterval, LoopCountUntil, newOnComplete, newOnUpdate, newOnFinished, newUsesRealTime, newExecuteOnStart);
        }
    }
}
