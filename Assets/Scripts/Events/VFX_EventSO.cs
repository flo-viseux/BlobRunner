using System;
using UnityEngine;


[CreateAssetMenu(fileName = "VFX_Event", menuName = "Events/New VFX Event", order = 0)]
public class VFX_EventSO : ScriptableObject
{
    public event Action<Vector3, VFX_Manager.EType, bool> OnEffectRaise;
    public event Action<VFX_Manager.EType> OnStopLoop;

    public void RaiseEvent(Vector3 position, VFX_Manager.EType vfxType, bool isLooping = false)
    {
        OnEffectRaise?.Invoke(position, vfxType, isLooping);
    }

    public void RaiseStopLoop(VFX_Manager.EType vfxType)
    {
        OnStopLoop?.Invoke(vfxType);
    }
}
