using System;
using UnityEngine;


    [CreateAssetMenu(fileName = "SFX_Event", menuName = "Events/New SFX Event", order = 0)]
    public class SFX_EventSO : ScriptableObject
    {
        public event Action<AudioManager.EType> OnJumpEffectRaise;
        public event Action<AudioManager.ETypeDive> OnDiveEffectRaise;

        public void RaiseJumpEvent(AudioManager.EType sfxJumpType)
        {
            OnJumpEffectRaise?.Invoke(sfxJumpType);
        }
        
        public void RaiseDiveEvent(AudioManager.ETypeDive sfxDiveType)
        {
            OnDiveEffectRaise?.Invoke(sfxDiveType);
        }
    }
