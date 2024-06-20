using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Manager : MonoBehaviour
{
    [SerializeField] private ParticleSystem bounceVFX;
    [SerializeField] private ParticleSystem diveVFX;

    private void OnEnable()
    {
        // TODO : create player event to play vfx
    }

    private void OnDisable()
    {
        // TODO : create player event to play vfx
    }

    public void EnableBounceVFX(Vector3 position)
    {
        bounceVFX.transform.position = position;
        bounceVFX.Play();
    }

    public void EnableDiveVFX(Vector3 position)
    {
        diveVFX.transform.position = position;
        diveVFX.Play();
    }
}
