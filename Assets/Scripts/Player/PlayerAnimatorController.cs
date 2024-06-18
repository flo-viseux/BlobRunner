using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private int ID_JumpTrigger = Animator.StringToHash("JumpTrigger");
    private int ID_LandTrigger = Animator.StringToHash("LandTrigger");

    private static int invulnerableHash = Animator.StringToHash("IsInvulnerable");

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Jump()
    {
        animator.ResetTrigger(ID_LandTrigger);
        animator.SetTrigger(ID_JumpTrigger);
    }

    public void Land()
    {
        animator.ResetTrigger(ID_JumpTrigger);
        animator.SetTrigger(ID_LandTrigger);
    }

    public void SetInvulnerable(bool value)
    {
        animator.SetBool(invulnerableHash, value);
    }
}
