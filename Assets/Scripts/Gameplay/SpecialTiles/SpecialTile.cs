using UnityEngine;

public class SpecialTile : MonoBehaviour
{
    #region Serialized fields
    [SerializeField] private SpecialTileEffect effect = null;
    #endregion

    #region Attributes
    private bool triggered = false;
    #endregion

    #region API
    public void Triggered()
    {
        if (triggered)
            return;

        triggered = true;

        effect.Effect();
    }

    public void Init()
    {
        triggered = false;

        if(effect)
            effect.Rebind();
    }
    #endregion

    #region UnityMethods
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
            Triggered();
    }
    #endregion
}
