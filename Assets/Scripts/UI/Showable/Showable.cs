using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Showable : MonoBehaviour
{
    public enum DefaultState { Show, Hide, Shown, Hidden }

    #region Constants
    static public readonly int showHash = Animator.StringToHash("show");
    static public readonly int hideHash = Animator.StringToHash("hide");
    static public readonly int instantHash = Animator.StringToHash("instant");
    #endregion

    #region SerializeFields
    [SerializeField] private Animator animator = null;

    [SerializeField] private DefaultState defaultState = DefaultState.Hidden;
    #endregion

    #region Attributes
    private bool isShowing = false;

    private bool isHiding = false;

    private bool isShown = false;

    private bool isHidden = false;

    private HashSet<Showable> children = new HashSet<Showable>();

    private bool isInitialized = false;
    #endregion

    #region API
    public DefaultState StateDefault => defaultState;

    public event Action OnWillShow;

    public event Action OnShown;

    public event Action OnWillHide;

    public event Action OnHidden;

    public void Show(bool instant = false)
    {
        if (isShown)
            return;

        if (!isShowing)
        {
            OnWillShow?.Invoke();
            ChildrenBroadcast(DefaultState.Show);
        }
        else
            instant = true;

        isShowing = true;
        isHidden = false;
        isHiding = false;

        InternalShow();

        if (isShown && !instant)
            return;

        animator.enabled = true;
        animator.SetBool(instantHash, instant);
        animator.SetTrigger(showHash);
    }

    public void Hide(bool instant = false)
    {
        if (isHidden)
            return;

        if (!isHiding)
        {
            OnWillHide?.Invoke();
            ChildrenBroadcast(DefaultState.Hide);
        }
        else
            instant = true;

        isHiding = true;
        isShown = false;
        isShowing = false;

        InternalHide();

        if (isHidden && !instant)
            return;

        animator.enabled = true;
        animator.SetBool(instantHash, instant);
        animator.SetTrigger(hideHash);
    }

    public virtual bool IsVisible => isHidden == false;

    public bool IsShow => isShowing || isShown;
    #endregion

    #region Unity methods
    private void Awake()
    {
        animator.keepAnimatorStateOnDisable = true;

        Showable parent = GetNearestParent();

        if (parent)
            parent.RegisterChild(this);
    }

    private void Start()
    {
        isInitialized = true;

        switch (defaultState)
        {
            case DefaultState.Show:
                InitAnimation("Show");
                Show();
                break;

            case DefaultState.Hide:
                InitAnimation("Hide");
                Hide();
                break;

            case DefaultState.Shown:
                Show(true);
                break;

            case DefaultState.Hidden:
                Hide(true);
                break;
        }
    }
    #endregion

    #region Private
    private void InitAnimation(string stateName)
    {
        animator.Play(stateName, 0, 0f);
        animator.WriteDefaultValues();

        RuntimeAnimatorController runtimeAnimator = animator.runtimeAnimatorController;
        for (int i = 0; i < runtimeAnimator.animationClips.Length; i++)
        {
            string[] split = runtimeAnimator.animationClips[i].name.Split(' ');

            if (split.Contains(stateName))
                runtimeAnimator.animationClips[i].SampleAnimation(this.gameObject, 0f);
        }
    }

    private Showable GetNearestParent()
    {
        Transform t = transform.parent;

        while (t != null)
        {
            Showable parent = t.GetComponent<Showable>();

            if (parent)
                return parent;
            else
                t = t.parent;
        }

        return null;
    }

    private void RegisterChild(Showable child)
    {
        children.Add(child);
    }

    private void ChildrenBroadcast(DefaultState state)
    {
        foreach (Showable child in children)
        {
            if (!child.IsVisible)
                continue;

            switch (state)
            {
                case DefaultState.Shown:
                    child.OnShown?.Invoke();
                    break;

                case DefaultState.Show:
                    child.OnWillShow?.Invoke();
                    break;

                case DefaultState.Hidden:
                    child.OnHidden?.Invoke();

                    break;

                case DefaultState.Hide:
                    child.OnWillHide?.Invoke();
                    break;
            }

            child.ChildrenBroadcast(state);
        }
    }
    #endregion

    #region StateMachine events
    public void Shown()
    {
        isShown = true;
        isShowing = false;

        InternalShown();

        OnShown?.Invoke();
        ChildrenBroadcast(DefaultState.Shown);
    }

    public void Hidden()
    {
        isHidden = true;
        isHiding = false;

        InternalHidden();

        OnHidden?.Invoke();
        ChildrenBroadcast(DefaultState.Hidden);
    }
    #endregion

    #region Internal
    protected virtual void InternalShow() { }

    protected virtual void InternalShown() { }

    protected virtual void InternalHide() { }

    protected virtual void InternalHidden() { }
    #endregion

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        if (!animator)
            animator = GetComponent<Animator>();
    }
#endif
}