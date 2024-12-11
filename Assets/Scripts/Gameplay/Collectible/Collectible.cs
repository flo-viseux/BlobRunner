using UnityEngine;

public class Collectible : MonoBehaviour
{
    #region Serialized fields
    [SerializeField] private CollectibleRenderer renderer = null;

    [SerializeField] private CollectibleEffect effect = null;
    #endregion

    #region Attributes
    private bool picked = false;
    #endregion

    #region API
    public void Picked()
    {
        if (picked)
            return;

        picked = true;

        if (effect != null)
            effect.Effect();

        renderer.Picked();
    }

    public void Init()
    {
        picked = false;
        renderer.Init();
    }
    #endregion

    #region UnityMethods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
            Picked();
    }
    #endregion
}
