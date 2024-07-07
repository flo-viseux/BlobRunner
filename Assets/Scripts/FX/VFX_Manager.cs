using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.Player;

public class VFX_Manager : MonoBehaviour
{
    public enum EType
    {
        Bounce,
        Dive,
    }

    [SerializeField] private VFX_EventSO VFX_Event;
    [SerializeField] private ParticleSystem bounceVFX;
    [SerializeField] private ParticleSystem diveVFX;

    private ParticleSystem bounceVFXInstance;
    private ParticleSystem diveVFXInstance;

    private void OnEnable()
    {
        VFX_Event.OnEffectRaise += LaunchEffect;

        bounceVFXInstance = Instantiate(bounceVFX, new Vector3(-100f,0f,0f),Quaternion.identity);
        diveVFXInstance = Instantiate(diveVFX, new Vector3(-100f,0f,0f),Quaternion.identity);
    }

    private void OnDisable()
    {
        VFX_Event.OnEffectRaise -= LaunchEffect;
    }

    private void LaunchEffect(Vector3 position, EType vfxType)
    {
        switch (vfxType)
        {
            case EType.Bounce : 
                EnableBounceVFX(position);
                return;
            case EType.Dive :
                EnableDiveVFX(position);
                return;
            default: return;
        }
    }

    private void EnableBounceVFX(Vector3 position)
    {
        bounceVFXInstance.transform.position = position;
        bounceVFXInstance.Play();
    }

    private void EnableDiveVFX(Vector3 position)
    {
        diveVFXInstance.transform.position = position;
        diveVFXInstance.Play();
    }
}
