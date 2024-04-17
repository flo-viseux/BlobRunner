using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInput : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float maxTimeHold = 5f;
    [Header("Size")]
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private float coefSize = 2f;
    [Header("Physic")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce = 10f;

    private float startTime;
    private bool isJumping;
    private float durationTouchHold;

    private void Start()
    {
        startTime = 0;
        isJumping = false;
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += GetSmaller;
        inputManager.OnEndTouch += GetBackToNormalSize;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= GetSmaller;
        inputManager.OnEndTouch -= GetBackToNormalSize;
    }
    
    public void GetSmaller(float time)
    {
        Debug.Log("get smaller");
        
        // change size
        Vector3 scale = spriteTransform.localScale;
        scale.y = scale.y / coefSize;
        spriteTransform.localScale = scale;

        // jump
        startTime = time;
    }

    public void GetBackToNormalSize(float time)
    {
        Debug.Log("get back to normal");
        
        // change size
        Vector3 scale = spriteTransform.localScale;
        scale.y = scale.y * coefSize;
        spriteTransform.localScale = scale;
        
        // jump
        durationTouchHold = Mathf.Clamp01((time - startTime) / maxTimeHold);
        Debug.Log($"hold time : {durationTouchHold}");
        isJumping = true;
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            Vector2 forceDirection = (jumpForce * durationTouchHold) * Vector2.up;
            rb.AddForce(forceDirection, ForceMode2D.Impulse);
            isJumping = false;
        }
    }
}
