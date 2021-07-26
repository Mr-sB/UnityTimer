using System;
using UnityEngine;

namespace GameUtil
{
    public class LoopUntilTimer : LoopTimer
    {
        protected Func<LoopUntilTimer, bool> _loopUntilFunc;
        private Action _onFinished;

        public LoopUntilTimer(bool isPersistence, float interval, Func<LoopUntilTimer, bool> loopUntil, Action onComplete,
            Action<float> onUpdate, Action onFinished, bool usesRealTime, bool executeOnStart, MonoBehaviour autoDestroyOwner)
            : base(isPersistence, interval, onComplete, onUpdate, usesRealTime, executeOnStart, autoDestroyOwner)
        {
            _loopUntilFunc = loopUntil;
            _onFinished = onFinished;
        }

        protected override void OnComplete()
        {
            if (_loopUntilFunc == null || !SafeCall(_loopUntilFunc, this)) return;
            //LoopTimer completed!
            isCompleted = true;
            SafeCall(_onFinished);
        }

        public virtual void Restart(float newInterval, Func<LoopUntilTimer, bool> newLoopUntil, Action newOnComplete, Action<float> newOnUpdate, Action newOnFinished, bool newUsesRealTime, bool newExecuteOnStart)
        {
            _loopUntilFunc = newLoopUntil;
            _onFinished = newOnFinished;
            Restart(newInterval, newOnComplete, newOnUpdate, newUsesRealTime, newExecuteOnStart);
        }
    }
}
