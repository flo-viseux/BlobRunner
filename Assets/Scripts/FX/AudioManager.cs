using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.Player;

public class AudioManager : MonoBehaviour
{
    public enum EType
    {
        Small = 0,
        Medium,
        High,
        HitGround
    }

    public enum ETypeDive
    {
        Dive,
        HitGround
    }

    [SerializeField] private SFX_EventSO SFX_Event;
    
    [Tooltip("Small, Medium, High")]
    [SerializeField] private AudioClip[] bounceSFX;
    [Tooltip("Small, Medium, High, HitGround")]
    [SerializeField] private AudioClip[] jumpSFX;
    [Tooltip("Dive, HitGround")]
    [SerializeField] private AudioClip[] diveSFX;

    [SerializeField] private AudioSource _source;

    private void OnEnable()
    {
        SFX_Event.OnJumpEffectRaise += PlayJump;
        SFX_Event.OnDiveEffectRaise += PlayDive;
    }

    private void OnDisable()
    {
        SFX_Event.OnJumpEffectRaise -= PlayJump;
        SFX_Event.OnDiveEffectRaise -= PlayDive;
    }

    private void PlayBounce(EType type)
    {
        _source.PlayOneShot(bounceSFX[(int)type]);
    }

    private void PlayJump(EType type)
    {
        _source.clip = jumpSFX[(int)type];
        _source.PlayOneShot(jumpSFX[(int)type]);
    }

    private void PlayDive(ETypeDive type)
    {
        _source.clip = diveSFX[(int)type];
        _source.PlayOneShot(diveSFX[(int)type]);
    }
}
