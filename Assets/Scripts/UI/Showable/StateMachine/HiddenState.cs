using UnityEngine;

public class HiddenState : StateMachineBehaviour
{
    #region Attributes
    private Showable showable = null;
    #endregion

    #region Unity methods
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!showable)
            showable = animator.GetComponent<Showable>();

        // If animated not in same object as Animator, it then takes in the nearest child.
        // Useful to not link the Animator with Showable.
        if (!showable)
            showable = animator.GetComponentInChildren<Showable>();

        animator.Update(1f);

        showable.Hidden();
    }
    #endregion
}
