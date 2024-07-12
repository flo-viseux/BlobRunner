using System;
using UnityEngine;


[CreateAssetMenu(fileName = "BoolEvent", menuName = "Events/New Bool Event", order = 0)]
public class BoolEventSO : ScriptableObject
{
    public event Action<bool> OnEventRaised;

    public void RaiseEvent(bool value)
    {
        OnEventRaised?.Invoke(value);
    }
}
