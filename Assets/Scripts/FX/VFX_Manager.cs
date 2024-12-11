using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.Player;

public class VFX_Manager : MonoBehaviour
{
    [SerializeField] private ParticleSystem bounceVFX;
    [SerializeField] private ParticleSystem diveVFX;

    private ParticleSystem bounceVFXInstance;
    private ParticleSystem diveVFXInstance;

    private void OnEnable()
    {
        // TODO : create player event to play vfx
        // Controller.onBounce += EnableBounceVFX;
        // Controller.onDiveStart += EnableDiveVFX;

        bounceVFXInstance = Instantiate(bounceVFX, new Vector3(-10f,0f,0f),Quaternion.identity);
        diveVFXInstance = Instantiate(diveVFX, new Vector3(-10f,0f,0f),Quaternion.identity);
    }

    private void OnDisable()
    {
        // TODO : create player event to play vfx
        // Controller.onBounce -= EnableBounceVFX;
        // Controller.onDiveStart -= EnableDiveVFX;
    }

    public void EnableBounceVFX(Vector3 position)
    {
        bounceVFXInstance.transform.position = position;
        bounceVFXInstance.Play();
    }

    public void EnableDiveVFX(Vector3 position)
    {
        diveVFXInstance.transform.position = position;
        diveVFXInstance.Play();
    }
}
