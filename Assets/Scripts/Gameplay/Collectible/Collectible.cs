using UnityEngine;

public class Collectible : MonoBehaviour
{
    #region Serialized fields
    [SerializeField] private CollectibleRenderer renderer = null;
    #endregion

    #region Attributes
    private bool picked;
    #endregion

    #region API
    public void Picked()
    {
        if (picked)
            return;

        picked = true;

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
        {
            Picked();
            Debug.Log("Player Collectible");
        }
    }
    #endregion
}
