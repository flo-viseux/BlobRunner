using UnityEngine;

public class BreakableGlassRenderer : MonoBehaviour
{
    #region Constants
    private static int triggeredVar = Animator.StringToHash("triggered");
    #endregion

    #region Serialized fields
    [SerializeField] private Animator animator = null;
    #endregion

    #region API
    public void Triggered()
    {
        animator.SetTrigger(triggeredVar);
    }

    public void Init()
    {
        animator.Rebind();
    }
    #endregion
}
