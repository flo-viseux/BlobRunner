using System;
using UnityEngine;


[CreateAssetMenu(fileName = "SimpleEvent", menuName = "Events/New Simple Event", order = 0)]
public class SimpleEventSO : ScriptableObject
{
    public event Action OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}
