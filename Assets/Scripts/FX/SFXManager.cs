using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.Player;
using Random = UnityEngine.Random;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioSource _sourceLoop;
    [SerializeField] private AudioPitchRun _pitchRun;
    [Header("SFX")]
    [SerializeField] private AudioClip[] bounceClips;
    [SerializeField] private AudioClip[] deathClips;
    [SerializeField] private AudioClip[] diveClips;
    [SerializeField] private AudioClip[] jumpClips;
    [SerializeField] private AudioClip[] jumpStepClips;
    [SerializeField] private AudioClip runClip;
    [SerializeField] private AudioClip shrinkClip;
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
        EventManager.JumpStepEvent += PlayJumpStep;
        EventManager.WinJumpStepEvent += PlayWinJumpStep;
        EventManager.ShrinkEvent += PlayShrink;
    }

    private void OnDisable()
    {
        EventManager.RunEvent -= PlayRun;
        EventManager.BounceEvent -= PlayBounce;
        EventManager.DiveEvent -= PlayDive;
        EventManager.DeathEvent -= PlayDeath;
        EventManager.JumpEvent -= PlayJump;
        EventManager.HitGroundEvent -= PlayHitGround;
        EventManager.JumpStepEvent -= PlayJumpStep;
        EventManager.WinJumpStepEvent -= PlayWinJumpStep;
        EventManager.ShrinkEvent -= PlayShrink;
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
        _pitchRun.Play();
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
    private void PlayJumpStep(int step, Vector3 position)
    {
        AudioClip clip = jumpClips[step];
        _source.PlayOneShot(clip);
    }
    
    private void PlayWinJumpStep(int step)
    {
        AudioClip clip = jumpClips[step];
        _source.PlayOneShot(clip);
    }
    
    private void PlayShrink()
    {
        _source.PlayOneShot(shrinkClip);
    }
}
