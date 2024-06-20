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
    
    [Tooltip("Small, Medium, High")]
    [SerializeField] private AudioClip[] bounceSFX;
    [Tooltip("Small, Medium, High, HitGround")]
    [SerializeField] private AudioClip[] jumpSFX;
    [Tooltip("Dive, HitGround")]
    [SerializeField] private AudioClip[] diveSFX;

    [SerializeField] private AudioSource _source;

    private void OnEnable()
    {
        Controller.OnDiveSFX += PlayDive;
    }

    private void OnDisable()
    {
        Controller.OnDiveSFX -= PlayDive;
    }

    private void PlayBounce(EType type)
    {
        _source.PlayOneShot(bounceSFX[(int)type]);
    }

    private void PlayJump(EType type)
    {
        _source.PlayOneShot(jumpSFX[(int)type]);
    }

    private void PlayDive(ETypeDive type)
    {
        _source.PlayOneShot(diveSFX[(int)type]);
        Debug.Log($"Play sound : {type.ToString()}");
    }
}
