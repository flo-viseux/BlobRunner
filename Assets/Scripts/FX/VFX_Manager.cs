using System;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Manager : MonoBehaviour
{
    public enum EType
    {
        Bounce,
        Jump,
        JumpStep,
        Dive,
        Collect,
        Victory,
        Death,
    }

    [SerializeField] private VFX_EventSO VFX_Event;

    [Header("VFX Player")]
    [SerializeField] private ParticleSystem bounceVFX_prefab;
    [SerializeField] private ParticleSystem jumpVFX_prefab;
    [SerializeField] private ParticleSystem jumpStepVFX_prefab;
    [SerializeField] private ParticleSystem diveVFX_prefab;
    [Header("VFX Collectible")]
    [SerializeField] private ParticleSystem collectVFX_prefab;
    [Header("VFX End Game")]
    [SerializeField] private ParticleSystem victoryVFX_prefab;
    [SerializeField] private ParticleSystem deathVFX_prefab;

    private ParticleSystem bounceVFX;
    private ParticleSystem jumpVFX;
    private ParticleSystem jumpStepVFX;
    private ParticleSystem diveVFX;
    private ParticleSystem collectVFX;
    private ParticleSystem victoryVFX;
    private ParticleSystem deathVFX;
    private const int NB_VFX = 7;

    private Dictionary<EType, ParticleSystem> allVFX = new Dictionary<EType, ParticleSystem>(NB_VFX);
    private bool hasWonLevel = false;
    
    private void OnEnable()
    {
        VFX_Event.OnEffectRaise += LaunchEffect;
        VFX_Event.OnStopLoop += StopParticle;

        if (allVFX.Count == 0 ) { InitVFX(); }
    }

    private void OnDisable()
    {
        VFX_Event.OnEffectRaise -= LaunchEffect;
        VFX_Event.OnStopLoop -= StopParticle;

        hasWonLevel = false;
    }

    private void InitVFX()
    {
        bounceVFX = Instantiate(bounceVFX_prefab, new Vector3(-100f, 0f, 0f), Quaternion.identity, transform);
        bounceVFX.name = "VFX_Bounce";
        allVFX.Add(EType.Bounce, bounceVFX);

        jumpVFX = Instantiate(jumpVFX_prefab, new Vector3(-100f, 0f, 0f), Quaternion.identity, transform);
        jumpVFX.name = "VFX_Jump";
        allVFX.Add(EType.Jump, jumpVFX);

        jumpStepVFX = Instantiate(jumpStepVFX_prefab, new Vector3(-100f, 0f, 0f), Quaternion.identity, transform);
        jumpStepVFX.name = "VFX_JumpStep";
        allVFX.Add(EType.JumpStep, jumpStepVFX);

        diveVFX = Instantiate(diveVFX_prefab, new Vector3(-100f, 0f, 0f), Quaternion.identity, transform);
        diveVFX.name = "VFX_Dive";
        allVFX.Add(EType.Dive, diveVFX);

        collectVFX = Instantiate(collectVFX_prefab, new Vector3(-100f, 0f, 0f), Quaternion.identity, transform);
        collectVFX.name = "VFX_Collect";
        allVFX.Add(EType.Collect, collectVFX);

        victoryVFX = Instantiate(victoryVFX_prefab, new Vector3(-100f, 0f, 0f), Quaternion.identity, transform);
        victoryVFX.name = "VFX_Victory";
        allVFX.Add(EType.Victory, victoryVFX);

        deathVFX = Instantiate(deathVFX_prefab, new Vector3(-100f, 0f, 0f), Quaternion.identity, transform);
        deathVFX.name = "VFX_Death";
        allVFX.Add(EType.Death, deathVFX);
    }


    private void LaunchEffect(Vector3 position, EType vfxType, bool isLooping)
    {
        if (hasWonLevel) { return; }

        if (allVFX.ContainsKey(vfxType))
        {
            allVFX[vfxType].transform.position = position;
            var mainParticle = allVFX[vfxType].main;
            mainParticle.loop = isLooping;
            allVFX[vfxType].Play();

            if (vfxType == EType.Victory) hasWonLevel = true;
        }
    }

    private void StopParticle(EType vfxType)
    {
        if (allVFX.ContainsKey(vfxType))
        {
            allVFX[vfxType].Stop();
        }
    }
}
