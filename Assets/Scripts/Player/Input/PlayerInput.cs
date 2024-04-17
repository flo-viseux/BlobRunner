using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private SO_PlayerDatas playerDatas;
    [Header("Input")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float maxTimeHold = 5f;
    [Header("Size")]
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private float coefSize = 2f;
    [Header("Physic")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float groundCheckDistance = 0.5f;
    [SerializeField] private LayerMask groundMask;

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
        inputManager.OnSwipeSuccessful += DashDown;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= GetSmaller;
        inputManager.OnEndTouch -= GetBackToNormalSize;
        inputManager.OnSwipeSuccessful -= DashDown;
    }
    
    public void GetSmaller(float time)
    {
        if (IsThereGround())
        {
            // Debug.Log("on ground : get smaller");

            playerDatas.State = PlayerState.SMALL;
            
            // change size
            Vector3 scale = spriteTransform.localScale;
            scale.y = scale.y / coefSize;
            spriteTransform.localScale = scale;

            // jump
            startTime = time;
        }
    }

    public void GetBackToNormalSize(float time)
    {
        if (playerDatas.State == PlayerState.SMALL)
        {
            // Debug.Log("is small : get back to normal");

            playerDatas.State = PlayerState.NORMAL;
            
            // change size
            Vector3 scale = spriteTransform.localScale;
            scale.y = scale.y * coefSize;
            spriteTransform.localScale = scale;
            
            // jump
            durationTouchHold = Mathf.Clamp01((time - startTime) / maxTimeHold);
            // Debug.Log($"hold time : {durationTouchHold}");
            isJumping = true;
        }
    }

    public void DashDown()
    {
        if (!IsThereGround())
        {
            Debug.Log("Dash Down");
        }
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

    private bool IsThereGround()
    {
        Vector2 startpos = rb.transform.position;
        Vector2 direction = startpos + Vector2.down;
        RaycastHit2D hit = Physics2D.Raycast(startpos, direction, groundCheckDistance, groundMask);
        Debug.DrawLine(startpos, direction * groundCheckDistance, Color.cyan, 0.5f);
        
        if (hit) Debug.Log($"collider detected : {hit.collider.name}");
        
        return hit != null;
    }
}
