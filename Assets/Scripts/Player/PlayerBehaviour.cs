using System;
using EZCameraShake;
using Player;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private SO_PlayerDatas playerDatas;
    [Header("Input")]
    [SerializeField] private InputManager inputManager;

    [SerializeField] private float minTimeHold = 0.5f;
    [SerializeField] private float maxTimeHold = 1.5f;
    [Header("Animation")] 
    [SerializeField] private PlayerAnimatorController animController;
    [Header("Size")]
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private float coefSize = 2f;
    [Header("Physic")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce = 12.5f;


    [SerializeField] private float jumpForceFromMinHold = 12.5f; //=jumpForce
    [SerializeField] private float jumpForceFromDefaultHold = 35f;
    [SerializeField] private float jumpForceFromMaxHold = 35f;
    [SerializeField] private AnimationCurve jumpCurve = null;
    [SerializeField] private bool usingCurve = false;


    [SerializeField] private float dashDownForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.5f;
    [SerializeField] private LayerMask groundMask;
    [Header("Bounce")] 
    [SerializeField] private int bounceTotalCount = 3;

    private float startTime;
    private bool isInAir;
    private float durationTouchHold;

    private bool isBouncing = false;
    private int bounceCount = 0;
    private float startSpeedFromHold = 0f;

    private bool isDashing = false;

    private void Start()
    {
        startTime = 0;
        isInAir = false;
        isDashing = false;
        bounceCount = 0;
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
            durationTouchHold = Mathf.Clamp((time - startTime), minTimeHold, maxTimeHold);

            if (durationTouchHold == minTimeHold)
                startSpeedFromHold = jumpForceFromMinHold;
            else if (durationTouchHold == maxTimeHold)
                startSpeedFromHold = jumpForceFromMaxHold;
            else
            {
                if (usingCurve)
                    startSpeedFromHold = jumpCurve.Evaluate(durationTouchHold);
                else
                    startSpeedFromHold = jumpForceFromDefaultHold;
            }
                

            Vector2 forceDirection = startSpeedFromHold * Vector2.up;
            rb.AddForce(forceDirection, ForceMode2D.Impulse);

            Debug.Log("GetBackToNormalSize");
            isBouncing = true;
            animController.Jump();
            bounceCount = 0;
            CameraShaker.Instance.ShakeOnce(.1f, .1f, .1f, .1f);
        }
    }

    public void DashDown()
    {
        if (!IsThereGround())
        {
            // Debug.Log("Dash Down");
            isInAir = true;
            isDashing = true;
            Vector2 dashDirection = dashDownForce * Vector2.down;
            rb.AddForce(dashDirection, ForceMode2D.Impulse);
        }
    }

    public void Jump()
    {
        if (!IsThereGround())
            return;
        
        // Debug.Log("Jump");
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        animController.Jump();
        //isInAir = true;
    }
    
    private bool IsThereGround()
    {
        Vector2 startpos = rb.transform.position;
        Vector2 direction = Vector2.down;
        RaycastHit2D hit = Physics2D.Raycast(startpos, direction, groundCheckDistance, groundMask);
        //Debug.DrawRay(startpos, direction * groundCheckDistance, Color.cyan, 0.5f);
        
        // if (hit.collider != null) Debug.Log($"Raycast > collider detected : {hit.collider.name}");
        // else Debug.Log("no collider");
        
        return hit.collider != null;
    }

    private void Update()
    {
        if (isInAir && IsThereGround())
        {
            isInAir = false;
            animController.Land();
        }

        if (!IsThereGround())
            isInAir = true;

        if (isDashing && IsThereGround() && isBouncing)
        {
            isInAir = false;
            isBouncing = false;
            isDashing = false;

            animController.Land();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isBouncing && !IsThereGround() && other.gameObject.layer == 6)
        {
            bounceCount++;
            float ratio = 1 - Mathf.Clamp01(bounceCount / (float)bounceTotalCount);
            if (bounceCount == bounceTotalCount+1)
            {
                isBouncing = false;
                return;
            }
            Vector2 direction = other.contacts[0].normal;
            rb.AddForce(direction * startSpeedFromHold * ratio, ForceMode2D.Impulse);
        }
    }
}
