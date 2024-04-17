using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private TouchController _touchController;
    
    public delegate void StartTouchEvent(float time);
    public event StartTouchEvent OnStartTouch;
    
    public delegate void EndTouchEvent(float time);
    public event EndTouchEvent OnEndTouch;

    private void Awake()
    {
        _touchController = new TouchController();
    }

    private void OnEnable()
    {
        _touchController.Enable();
    }

    private void OnDisable()
    {
        _touchController.Disable();
    }

    private void Start()
    {
        _touchController.Player.TouchPress.started += ctx => StartTouch(ctx);
        _touchController.Player.TouchPress.canceled += ctx => EndTouch(ctx);
    }
    
    private void StartTouch(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null)
        {
            OnStartTouch((float)context.startTime);
        }
    }
    
    private void EndTouch(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null)
        {
            OnEndTouch((float)context.time);
        }
    }
}
