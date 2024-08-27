using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3DAnimator : MonoBehaviour
{
    private int ID_ShrinkBool = Animator.StringToHash("ActiveShrink");
    private int ID_FallTrigger = Animator.StringToHash("IsFalling");
    private int ID_JumpTrigger = Animator.StringToHash("Jump");
    private int ID_DiveTrigger = Animator.StringToHash("Dive");
    private int ID_LandTrigger = Animator.StringToHash("Land");
    private int ID_DeathTrigger = Animator.StringToHash("Death");
    
    private Animator animator;

    private Coroutine resetTriggerRoutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FallAnimation()
    {
        animator.SetTrigger(ID_FallTrigger);
        resetTriggerRoutine = StartCoroutine(ResetTriggerCoroutine(ID_FallTrigger));
        GameManager.Instance.coroutineStorage.AddRoutine(resetTriggerRoutine);
    }

    public void LandingAnimation()
    {
        animator.SetTrigger(ID_LandTrigger);
        resetTriggerRoutine = StartCoroutine(ResetTriggerCoroutine(ID_LandTrigger));
        GameManager.Instance.coroutineStorage.AddRoutine(resetTriggerRoutine);
    }

    public void ShrinkAnimation()
    {
        animator.SetBool(ID_ShrinkBool, true);
    }

    public void EndShrinkAnimation()
    {
        animator.SetBool(ID_ShrinkBool, false);
    }

    public void JumpAnimation()
    {
        animator.SetTrigger(ID_JumpTrigger);
        resetTriggerRoutine = StartCoroutine(ResetTriggerCoroutine(ID_JumpTrigger));
        GameManager.Instance.coroutineStorage.AddRoutine(resetTriggerRoutine);
    }

    public void DiveAnimation()
    {
        animator.SetTrigger(ID_DiveTrigger);
        resetTriggerRoutine = StartCoroutine(ResetTriggerCoroutine(ID_DiveTrigger));
        GameManager.Instance.coroutineStorage.AddRoutine(resetTriggerRoutine);
    }
    
    public void DeathAnimation()
    {
        animator.SetTrigger(ID_DeathTrigger);
    }

    private IEnumerator ResetTriggerCoroutine(int id)
    {
        yield return new WaitForSeconds(0.1f);
        animator.ResetTrigger(id);
        GameManager.Instance.coroutineStorage.RemoveRoutine(resetTriggerRoutine);
    }
}
