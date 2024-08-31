using System;
using Runner.Player;
using UnityEngine;

public class BreakableGlassEffect : SpecialTileEffect
{
    #region SerializedFields
    [SerializeField] private Collider2D collider;

    [SerializeField] private BreakableGlassRenderer renderer;
    [SerializeField] private ParticleSystem breakGlassVFXprefab;
    #endregion

    private AudioSource _source;
    private ParticleSystem breakGlassVFX;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        breakGlassVFX = Instantiate(breakGlassVFXprefab, transform.position, Quaternion.identity, transform);
        breakGlassVFX.name = "VFX_BreakGlass";
    }

    #region API
    public override void Rebind()
    {
        collider.enabled = true;
        renderer.Init();
    }

    public override void Effect()
    {
        //TODO Check if player is dashing to the bottom
        if (Controller.GetCurrentState() != Controller.EState.Dive)
            return;

        collider.enabled = false;
        renderer.Triggered();
        if (_source != null) _source.PlayOneShot(_source.clip);
        breakGlassVFX.Play();
    }
    #endregion
}
