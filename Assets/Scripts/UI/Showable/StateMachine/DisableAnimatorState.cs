using UnityEngine;

public class DisableAnimatorState : StateMachineBehaviour
{
    #region SerializeFields
    [SerializeField] private bool onExit = false;
    #endregion

    #region Unity methods
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!onExit)
            animator.enabled = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (onExit)
            animator.enabled = false;
    }
    #endregion
}
