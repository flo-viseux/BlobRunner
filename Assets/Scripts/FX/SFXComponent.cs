using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXComponent : MonoBehaviour
{
    private AudioSource _source;
    
    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _source.Play();
    }

    private void OnDisable()
    {
        _source.Stop();
    }
}
