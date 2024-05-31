using UnityEngine;

public class BreakableGlassEffect : SpecialTileEffect
{
    #region SerializedFields
    [SerializeField] private Collider2D collider;

    [SerializeField] private BreakableGlassRenderer renderer;
    #endregion

    #region API
    public override void Rebind()
    {
        collider.enabled = true;
        renderer.Init();
    }

    public override void Effect()
    {
        //TODO Check if player is dashing to the bottom
        collider.enabled = false;
        renderer.Triggered();
    }
    #endregion
}
