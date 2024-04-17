using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float minimumSwipeMagnitude = 5f;
    private Vector2 swipeDirection;
    
    private TouchController _touchController;
    
    public delegate void StartTouchEvent(float time);
    public event StartTouchEvent OnStartTouch;
    
    public delegate void EndTouchEvent(float time);
    public event EndTouchEvent OnEndTouch;

    public delegate void SwipeSuccesful();

    public event SwipeSuccesful OnSwipeSuccessful;

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
        _touchController.Player.Swipe.performed += ctx => Swipe(ctx);
        
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

    private void Swipe(InputAction.CallbackContext context)
    {
        swipeDirection = context.ReadValue<Vector2>();
        bool swipeX = swipeDirection.x < 0.1f & swipeDirection.x > -0.1f;
        if (swipeDirection.y < 0 & swipeX)
        {
            Debug.Log(("Swipe down"));
            OnSwipeSuccessful();
        }
    }
}
