using UnityEngine;

public class Parallax : MonoBehaviour
{
    #region Serialized fields
    [SerializeField] protected float speed = 0.1f;
    #endregion

    #region API
    public bool Scrolling { get; set; }

    public bool Spawning { get; set; }
    #endregion

    #region Unity methods
    protected virtual void Awake()
    {
        Spawning = true;
    }
    #endregion
}
