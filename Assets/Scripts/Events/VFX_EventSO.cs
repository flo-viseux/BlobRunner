using System;
using UnityEngine;


    [CreateAssetMenu(fileName = "VFX_Event", menuName = "Events/New VFX Event", order = 0)]
    public class VFX_EventSO : ScriptableObject
    {
        public event Action<Vector3, VFX_Manager.EType> OnEffectRaise;

        public void RaiseEvent(Vector3 position, VFX_Manager.EType vfxType)
        {
            OnEffectRaise?.Invoke(position, vfxType);
        }
    }
