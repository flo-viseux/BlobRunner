using UnityEngine;

public class CollectibleRenderer : MonoBehaviour
{
    #region Constants
    private static int pickedVar = Animator.StringToHash("picked");
    private static int initVar = Animator.StringToHash("init");
    #endregion

    #region Serialized fields
    [SerializeField] private Animator animator = null;
    #endregion

    #region API
    public void Picked()
    {
        animator.SetTrigger(pickedVar);
    }

    public void Init()
    {
        animator.SetTrigger(initVar);
    }
    #endregion
}
