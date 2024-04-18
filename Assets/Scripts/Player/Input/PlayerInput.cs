using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private SO_PlayerDatas playerDatas;
    [Header("Input")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float maxTimeHold = 5f;
    [Header("Animation")] 
    [SerializeField] private PlayerAnimatorController animController;
    [Header("Size")]
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private float coefSize = 2f;
    [Header("Physic")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpForceFromHold = 10f;
    [SerializeField] private float minJumpForceFromHold = 0.2f;
    [SerializeField] private float dashDownForce = 5f;
    [SerializeField] private Transform feetTransform;
    [SerializeField] private float groundCheckDistance = 0.5f;
    [SerializeField] private LayerMask groundMask;

    private float startTime;
    private bool isInAir;
    private float durationTouchHold;
    private bool isThereBounce = false;

    private void Start()
    {
        startTime = 0;
        isInAir = false;
        isThereBounce = false;
    }

    private void OnEnable()
    {
        inputManager.OnSwipeSuccessful += DashDown;
        inputManager.OnTap += Jump;
        inputManager.OnStartTouch += GetSmaller;
        inputManager.OnEndTouch += GetBackToNormalSize;

    }
    
    private void OnDisable()
    {
        inputManager.OnSwipeSuccessful -= DashDown;
        inputManager.OnTap += Jump;
        inputManager.OnStartTouch -= GetSmaller;
        inputManager.OnEndTouch -= GetBackToNormalSize;
    }
    
    public void GetSmaller(float time)
    {
        if (IsThereGround())
        {
            playerDatas.State = PlayerState.SMALL;
            
            // change size
            Vector3 scale = spriteTransform.localScale / coefSize;
            spriteTransform.localScale = scale;

            // jump
            startTime = time;
        }
    }

    public void GetBackToNormalSize(float time)
    {
        if (playerDatas.State == PlayerState.SMALL)
        {
            playerDatas.State = PlayerState.NORMAL;
            
            // change size
            Vector3 scale = spriteTransform.localScale * coefSize;
            spriteTransform.localScale = scale;
            
            // jump
            durationTouchHold = Mathf.Clamp((time - startTime) / maxTimeHold, minJumpForceFromHold, 1f);
            Vector2 forceDirection = (jumpForceFromHold * durationTouchHold) * Vector2.up;
            rb.AddForce(forceDirection, ForceMode2D.Impulse);

            isThereBounce = true;
            // animController.EnterJumpTrig();
            isInAir = true;
        }
    }

    public void DashDown()
    {
        if (!IsThereGround())
        {
            Debug.Log("Dash Down");
            isInAir = true;
            Vector2 dashDirection = dashDownForce * Vector2.down;
            rb.AddForce(dashDirection, ForceMode2D.Impulse);
        }
    }

    public void Jump()
    {
        Debug.Log(isInAir);
        if (isInAir) return;
        Debug.Log("Jump");
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isInAir = true;
    }

    private void FixedUpdate()
    {
        if (IsThereGround())
        {
            isInAir = false;
        }
    }

    private bool IsThereGround()
    {
        Vector2 startpos = rb.transform.position;
        Vector2 direction = Vector2.down;
        RaycastHit2D hit = Physics2D.Raycast(startpos, direction, groundCheckDistance, groundMask);
        Debug.DrawRay(startpos, direction * groundCheckDistance, Color.cyan, 0.5f);
        
        if (hit.collider != null) Debug.Log($"Raycast > collider detected : {hit.collider.name}");
        else Debug.Log("no collider");
        
        return hit.collider != null;
    }
}
