using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.Player;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    // public enum EType
    // {
    //     Small = 0,
    //     Medium,
    //     High,
    //     HitGround
    // }
    //
    // public enum ETypeDive
    // {
    //     Dive,
    //     HitGround
    // }

    // [SerializeField] private SFX_EventSO SFX_Event;
    //
    // [Tooltip("Small, Medium, High")]
    // [SerializeField] private AudioClip[] bounceSFX;
    // [Tooltip("Small, Medium, High, HitGround")]
    // [SerializeField] private AudioClip[] jumpSFX;
    // [Tooltip("Dive, HitGround")]
    // [SerializeField] private AudioClip[] diveSFX;
    //
    // [SerializeField] private AudioSource _source;
    //
    // private void OnEnable()
    // {
    //     SFX_Event.OnJumpEffectRaise += PlayJump;
    //     SFX_Event.OnDiveEffectRaise += PlayDive;
    // }
    //
    // private void OnDisable()
    // {
    //     SFX_Event.OnJumpEffectRaise -= PlayJump;
    //     SFX_Event.OnDiveEffectRaise -= PlayDive;
    // }
    //
    // private void PlayBounce(EType type)
    // {
    //     _source.PlayOneShot(bounceSFX[(int)type]);
    // }
    //
    // private void PlayJump(EType type)
    // {
    //     _source.clip = jumpSFX[(int)type];
    //     _source.PlayOneShot(jumpSFX[(int)type]);
    // }
    //
    // private void PlayDive(ETypeDive type)
    // {
    //     _source.clip = diveSFX[(int)type];
    //     _source.PlayOneShot(diveSFX[(int)type]);
    // }

    [SerializeField] private AudioSource _source;
    [Header("SFX")]
    [SerializeField] private AudioClip[] bounceClips;
    [SerializeField] private AudioClip[] deathClips;
    [SerializeField] private AudioClip[] diveClips;
    [SerializeField] private AudioClip[] jumpClips;
    [SerializeField] private AudioClip runClip;
    [SerializeField] private AudioClip hitGroundClip;

    private int bounceLength;
    private int deathLength;
    private int diveLength;
    private int jumpLength;
    
    private void OnEnable()
    {
        EventManager.RunEvent += PlayRun;
        EventManager.BounceEvent += PlayBounce;
        EventManager.DiveEvent += PlayDive;
        EventManager.DeathEvent += PlayDeath;
        EventManager.JumpEvent += PlayJump;
        EventManager.HitGroundEvent += PlayHitGround;
    }

    private void OnDisable()
    {
        EventManager.RunEvent -= PlayRun;
        EventManager.BounceEvent -= PlayBounce;
        EventManager.DiveEvent -= PlayDive;
        EventManager.DeathEvent -= PlayDeath;
        EventManager.JumpEvent -= PlayJump;
        EventManager.HitGroundEvent -= PlayHitGround;
    }

    private void Start()
    {
        bounceLength = bounceClips.Length;
        deathLength = deathClips.Length;
        diveLength = diveClips.Length;
        jumpLength = jumpClips.Length;
    }

    private void PlayRun()
    {
        _source.PlayOneShot(runClip);
    }

    private void PlayBounce(Vector3 position)
    {
        AudioClip clip = bounceClips[Random.Range(0, bounceLength)];
        _source.PlayOneShot(clip);
    }

    private void PlayDive(Vector3 position)
    {
        AudioClip clip = diveClips[Random.Range(0, diveLength)];
        _source.PlayOneShot(clip);
    }

    private void PlayDeath()
    {
        AudioClip clip = deathClips[Random.Range(0, deathLength)];
        _source.PlayOneShot(clip);
    }

    private void PlayJump(Vector3 position)
    {
        AudioClip clip = jumpClips[Random.Range(0, jumpLength)];
        _source.PlayOneShot(clip);
    }

    private void PlayHitGround()
    {
        _source.PlayOneShot(hitGroundClip);
    }
}
