using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private int ID_isJumping = Animator.StringToHash("IsJuming");
    private int ID_isLanding = Animator.StringToHash("IsLanding");
    private int ID_JumpTrigger = Animator.StringToHash("JumpTrigger");
    private int ID_LandTrigger = Animator.StringToHash("LandTrigger");

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void EnterJumping()
    {
        animator.SetBool(ID_isJumping, true);
        // Debug.Log("enter jump");
    }

    public void ExitJumping()
    {
        animator.SetBool(ID_isJumping, false);
    }

    public void EnterJumpTrig()
    {
        animator.ResetTrigger(ID_LandTrigger);
        animator.SetTrigger(ID_JumpTrigger);
    }

    public void EnterLanding()
    {
        animator.SetBool(ID_isLanding, true);
        // Debug.Log("enter landing");
    }

    public void ExitLanding()
    {
        animator.SetBool(ID_isLanding, false);
    }
    
    public void EnterLandTrig()
    {
        animator.ResetTrigger(ID_JumpTrigger);
        animator.SetTrigger(ID_LandTrigger);
    }
}
