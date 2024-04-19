using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private enum InputState
    {
        NONE,
        TAP,
        HOLD,
        SWIPE
    }

    private InputState state;
    
    public delegate void StartTouchEvent(float time);
    public event StartTouchEvent OnStartTouch;
    
    public delegate void EndTouchEvent(float time);
    public event EndTouchEvent OnEndTouch;

    public delegate void SwipeSuccesfulEvent();
    public event SwipeSuccesfulEvent OnSwipeSuccessful;
    
    public delegate void TapEvent();
    public event TapEvent OnTap;
    
    private bool isHolding = false;

    private void Start()
    {
        state = InputState.NONE;
    }

    void OnEnable()
    {
        Lean.Touch.LeanTouch.OnFingerSwipe += HandleSwipe;
        Lean.Touch.LeanTouch.OnFingerTap += HandleTap;
        Lean.Touch.LeanTouch.OnFingerOld += HandleFingerOld;
        Lean.Touch.LeanTouch.OnFingerUp += HandleFingerUp;
    }

    void OnDisable()
    {
        Lean.Touch.LeanTouch.OnFingerSwipe -= HandleSwipe;
        Lean.Touch.LeanTouch.OnFingerTap -= HandleTap;
        Lean.Touch.LeanTouch.OnFingerOld -= HandleFingerOld;
        Lean.Touch.LeanTouch.OnFingerUp -= HandleFingerUp;
    }

    void HandleFingerOld(Lean.Touch.LeanFinger finger)
    {
        if (finger.Index != 0) return;
        
        if (state != InputState.NONE) return;
        
        // Debug.Log($"Hold {finger.Age} {finger.Index}");
        
        state = InputState.HOLD;
        
        if (finger.Age > 2f) return;
        OnStartTouch(finger.Age);
        isHolding = true;
    }

    void HandleFingerUp(Lean.Touch.LeanFinger finger)
    {
        if (finger.Index != 0) return;
        
        if (state == InputState.HOLD)
        {
            // Debug.Log($"Hold stop {finger.Age} {finger.Index}");
            OnEndTouch(finger.Age);
        }

        state = InputState.NONE;
    }

    void HandleSwipe(Lean.Touch.LeanFinger finger)
    {
        if (finger.Index != 0) return;
        
        if (state != InputState.NONE) return;
        
        Vector2 swipeDelta = finger.SwipeScreenDelta;
        
        if (swipeDelta.y < -Mathf.Abs(swipeDelta.x))
        {
            state = InputState.SWIPE;
            // Debug.Log("Swiped Down");
            // Debug.Log($"Swiped Down {finger.Age} {finger.Index}");
            OnSwipeSuccessful();
        }

        state = InputState.NONE;
    }

    void HandleTap(Lean.Touch.LeanFinger finger)
    {
        if (finger.Index != 0) return;
        
        if (state != InputState.NONE) return;

        state = InputState.TAP;
        // Debug.Log("Tap");
        // Debug.Log($" Tap {finger.Age} {finger.Index}");
        OnTap();
        
        state = InputState.NONE;
    }
}
