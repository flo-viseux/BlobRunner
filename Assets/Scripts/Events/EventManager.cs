using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventManager
{
    public static event Action HitHeadEvent;
    public static event Action HitGroundEvent;
    public static event Action<Vector3> BounceEvent;
    public static event Action RunEvent;
    public static event Action ShrinkEvent;
    public static event Action EndShrinkEvent;
    public static event Action<Vector3> JumpEvent;
    public static event Action<Vector3> JumpStepEvent;
    public static event Action<Vector3> DiveEvent;
    public static event Action DeathEvent;
 
    public static void RaiseHitHeadEvent() => HitHeadEvent?.Invoke();
    public static void RaiseHitGroundEvent() => HitGroundEvent?.Invoke();
    public static void RaiseBounceEvent(Vector3 position) => BounceEvent?.Invoke(position);
    public static void RaiseRunEvent() => RunEvent?.Invoke();
    public static void RaiseShrinkEvent() => ShrinkEvent?.Invoke();
    public static void RaiseEndShrinkEvent() => EndShrinkEvent?.Invoke();
    public static void RaiseJumpEvent(Vector3 position) => JumpEvent?.Invoke(position);
    public static void RaiseJumpStepEvent(Vector3 position) => JumpStepEvent?.Invoke(position);
    public static void RaiseDiveEvent(Vector3 position) => DiveEvent?.Invoke(position);
    public static void RaiseDeathEvent() => DeathEvent?.Invoke();
}
