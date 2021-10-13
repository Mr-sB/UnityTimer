# Unity Timer

Run actions after a delay in Unity3D.

This library has been battle-tested and hardened throughout numerous projects, including the award-winning [Pitfall Planet](http://pitfallplanet.com/).

Written by [Alexander Biggs](http://akbiggs.com) + [Adam Robinson-Yu](http://www.adamgryu.com/).

Fork by GH-ZJ(wustzhangjie@gmail.com)

## Basic Example

The Unity Timer package provides the following method for creating timers:
```c#
public static DelayTimer DelayAction(float duration, Action onComplete, Action<float> onUpdate = null, bool useRealTime = false, Object autoDestroyOwner = null);

public static DelayFrameTimer DelayFrameAction(int frame, Action onComplete, Action<float> onUpdate = null, Object autoDestroyOwner = null);

public static LoopTimer LoopAction(float interval, Action onComplete, Action<float> onUpdate = null, bool useRealTime = false, bool executeOnStart = false, Object autoDestroyOwner = null);
        
public static LoopUntilTimer LoopUntilAction(float interval, Func<LoopUntilTimer, bool> loopUntil, Action onComplete, Action<float> onUpdate = null, Action onFinished = null, bool useRealTime = false, bool executeOnStart = false, Object autoDestroyOwner = null);

public static LoopCountTimer LoopCountAction(float interval, int loopCount, Action onComplete, Action<float> onUpdate = null, Action onFinished = null, bool useRealTime = false, bool executeOnStart = false, Object autoDestroyOwner = null);

public static DelayTimer PersistenceDelayAction(float duration, Action onComplete, Action<float> onUpdate = null, bool useRealTime = false, Object autoDestroyOwner = null);

public static DelayFrameTimer PersistenceDelayFrameAction(int frame, Action onComplete, Action<float> onUpdate = null, Object autoDestroyOwner = null);

public static LoopTimer PersistenceLoopAction(float interval, Action onComplete, Action<float> onUpdate = null, bool useRealTime = false, bool executeOnStart = false, Object autoDestroyOwner = null);

public static LoopUntilTimer PersistenceLoopUntilAction(float interval, Func<LoopUntilTimer, bool> loopUntil, Action onComplete, Action<float> onUpdate = null, Action onFinished = null, bool useRealTime = false, bool executeOnStart = false, Object autoDestroyOwner = null);

public static LoopCountTimer PersistenceLoopCountAction(float interval, int loopCount, Action onComplete, Action<float> onUpdate = null, Action onFinished = null, bool useRealTime = false, bool executeOnStart = false, Object autoDestroyOwner = null);
```
## Motivation

Out of the box, without this library, there are two main ways of handling timers in Unity:

1. Use a coroutine with the WaitForSeconds method.
2. Store the time that your timer started in a private variable (e.g. `startTime = Time.time`), then check in an Update call if `Time.time - startTime >= timerDuration`.

The first method is verbose, forcing you to refactor your code to use IEnumerator functions. Furthermore, it necessitates having access to a MonoBehaviour instance to start the coroutine, meaning that solution will not work in non-MonoBehaviour classes. Finally, there is no way to prevent WaitForSeconds from being affected by changes to the [time scale](http://docs.unity3d.com/ScriptReference/Time-timeScale.html).

The second method is error-prone, and hides away the actual game logic that you are trying to express.

This library alleviates both of these concerns, making it easy to add an easy-to-read, expressive timer to any class in your Unity project.

## Features

* Make a timer repeat by call Timer.LoopAction.
```c#
private void Start()
{
   Timer.LoopAction(5, () => { Debug.LogError("Timer Called"); });
}
```

* Make a loop timer execute when start by setting `executeOnStart` to true.
```c#
private void Start()
{
   Timer.LoopAction(5, () => { Debug.LogError("Timer Called"); }, executeOnStart: true);
}
```

* Make a timer based on frame count by call Timer.DelayFrameAction.
```c#
private void Start()
{
   Timer.DelayFrameAction(5, () => { Debug.LogError("Timer Called"); });
}
```

* Measure time by [realtimeSinceStartup](http://docs.unity3d.com/ScriptReference/Time-realtimeSinceStartup.html) instead of scaled game time by setting `useRealTime` to true.
```c#
private void Start()
{
   Timer.DelayAction(5, () => { Debug.LogError("Timer Called"); }, useRealTime: true);
}
```

* Cancel a timer after calling it.
```c#
Timer timer;

void Start() {
   timer = Timer.LoopAction(5, () => { Debug.LogError("Timer Called"); });
}

void Update() {
   if (Input.GetKeyDown(KeyCode.X)) {
      Timer.Cancel(timer);
   }
}
```

* Attach the timer to a UnityEngine.Object by setting `autoDestroyOwner` to the UnityEngine.Object, so that the timer is destroyed when the UnityEngine.Object is.

Very often, a timer called from a Component will manipulate that component's state. Thus, it is common practice to cancel the timer in the OnDestroy method of the Component. We've added a convenient extension method that attaches a Timer to a Component such that it will automatically cancel the timer when the Component is detected as null.
```c#
public class CoolMonoBehaviour : MonoBehaviour {

   private void Start()
   {
      //The timer will cancel when the Component is destroyed;
      Timer.DelayAction(5, () => { Debug.LogError("Timer Called"); }, useRealTime: true, autoDestroyOwner: this);
   }

   private void Update()
   {
      // This code could destroy the object at any time!
      if (Input.GetKeyDown(KeyCode.X)) {
         GameObject.Destroy(this.gameObject);
      }
   }
}
```

* Update a value gradually over time using the `onUpdate` callback.

```c#
// Change a color from white to red over the course of five seconds.
Color color = Color.white;
float transitionDuration = 5f;

Timer.DelayAction(transitionDuration,
   onUpdate: secondsElapsed => color.r = 255 * (secondsElapsed / transitionDuration),
   onComplete: () => Debug.Log("Color is now red"));
```

* A number of other useful features are included!

- `timer.Pause()`
- `timer.Resume()`
- `timer.Cancel()`
- `timer.Restart()`
- `timer.GetTimeElapsed()`
- `timer.GetTimeRemaining()`
- `timer.GetRatioComplete()`
- `timer.isDone`
- `Timer.CancelAllRegisteredTimers()`
- `Timer.CancelAllRegisteredTimersByOwner(owner)`
- `Timer.PauseAllRegisteredTimers()`
- `Timer.ResumeAllRegisteredTimers()`

* Make a timer not effect by `Timer.XXXAllRegisteredTimers()` function by call `Timer.PersistenceXXX()` function.
```c#
Timer timer;

void Start() {
   //The timer will not cancel when Timer.XXXAllRegisteredTimers();
   timer = Timer.PersistenceLoopAction(5, () => { Debug.LogError("Timer Called"); });
}

void Update() {
   //No effect to timer
   if (Input.GetKeyDown(KeyCode.X))
      Timer.CancelAllRegisteredTimers();

   //Only this can cancel persistence timer
   if(Input.GetKeyDown(KeyCode.C))
      Timer.Cancel(timer);//same to timer?.Cancel();
}
```

* All timer generator functions can shortcut call by using Component/GameObject Extensions functions, and the timer will attach to the Component/GameObject so that the timer is destroyed when the Component/GameObject is.
```c#
void Start() {
   //The timer will attach to the Component instance.
   this.DelayAction(5, () => { Debug.LogError("Timer Called"); });
}
```


* A test scene + script demoing all the features is included with the package in the `/Example` folder.

## Usage Notes / Caveats
* All timers will not destroy when change scene, because TimerManager is `DontDestroyOnLoad`.
