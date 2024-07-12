using System;
using UnityEngine;


[CreateAssetMenu(fileName = "SliderEvent", menuName = "Events/New Slider Event", order = 0)]
public class SliderEventSO : ScriptableObject
{
    public event Action<float> OnInitialization;
    public event Action<float> OnUpdate;
    public event Action OnReset;

    public void RaiseInitEvent(float startValue)
    {
        OnInitialization?.Invoke(startValue);
    }

    public void RaiseUpdateEvent(float value)
    {
        OnUpdate?.Invoke(value);
    }

    public void RaiseResetEvent()
    {
        OnReset?.Invoke();
    }
}
