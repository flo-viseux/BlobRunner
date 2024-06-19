using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Runner.Player
{
    public class Controller : MonoBehaviour
    {

        public enum EState
        {
            Normal,
            Shrink,
            Jump,
            Bounce,
            Dive,
        }
        
        public static EState currentState;

        [Tooltip("Normal, Shrink, Bounce")]
        public Color[] stateColor;
        public SpriteRenderer _renderer;
        
        [Header("Components")]
        [SerializeField] private InputManager inputManager;
        [SerializeField] private Rigidbody2D rb2D;
        [SerializeField] private Transform spriteTransform;
        
        [Header("Ground")]
        [SerializeField] private float checkFloorDist;
        [SerializeField] private LayerMask groundMask;
        
        [Header("Jump")] 
        [Tooltip("x : time, y : jumpForce")]
        [SerializeField] private Vector2[] jumpSteps;

        [Header("Bounce")] 
        [SerializeField] private float bounceForceMin = 15f;
        [SerializeField] private float distanceFromGroundToTap = 0.2f;
        [SerializeField] private float addBounceForce = 0.1f;
        [SerializeField] private float reactionTime = 0.5f;
        
        [Header("Shrink")]
        [SerializeField] private float coefSize;
        
        [Header("Dive")]
        [SerializeField] private float diveForce;

        // hold
        private float startTimeHold;
        private float holdDuration;
        private float maxHoldTime;
        // ground
        private bool isOnGround;
        // tap input
        private bool HasTap = false;
        private bool CanTap = false;
        private Vector2 normal;
        private float timer;
        private float inputTapWindowDuration = 0.25f;
        // velocity
        private Vector2 previousVelocity;
        
        // UI : slider 
        private JumpChargeUI _jumpChargeUI = null;
        public JumpChargeUI GetJumpChargeUI() => _jumpChargeUI;
        
        
        private void Start()
        {
            currentState = EState.Normal;
            int lastIndex = jumpSteps.Length - 1;
            maxHoldTime = jumpSteps[lastIndex].x;
            HasTap = false;
            CanTap = false;
        }
        
        

        private void OnEnable()
        {
            inputManager.OnStartTouch += Shrink;
            inputManager.OnEndTouch += Jump;
            inputManager.OnSwipeSuccessful += Dive;
            inputManager.OnTap += JumpBack;
        }

        private void OnDisable()
        {
            inputManager.OnStartTouch -= Shrink;
            inputManager.OnEndTouch -= Jump;
            inputManager.OnSwipeSuccessful -= Dive;
            inputManager.OnTap -= JumpBack;
        }

        private void Update()
        {
            // debug
            UpdateColor();
            if (UIDebugText.Instance != null)
                UIDebugText.Instance.UpdateText(currentState.ToString());
            //----
            
            CheckGround();
            
            if (currentState == EState.Shrink)
            {
                if (holdDuration > maxHoldTime) return;
                holdDuration += Time.deltaTime;
                GetJumpChargeUI().UpdateCharge(holdDuration);
                return;
            }

            if (currentState == EState.Dive && isOnGround)
            {
                currentState = EState.Normal;
                return;
            }
        }
        
        // debug only
        private void UpdateColor()
        {
            switch (currentState)
            {
                case EState.Normal : _renderer.color = stateColor[0];
                    return;
                case EState.Shrink : _renderer.color = stateColor[1];
                    return;
                case EState.Bounce : _renderer.color = stateColor[2];
                    return;
                default: _renderer.color = Color.white;
                    return;
            }
        }

        private void FixedUpdate()
        {
            if (CanTap) return;
            previousVelocity = rb2D.velocity;
        }

        private void Jump(float endTime)
        {
            if (currentState == EState.Shrink)
            {
                // STOP SHRINK
                holdDuration = 0f;
                GetJumpChargeUI().ResetSlider();
                // switch scale back to normal
                Vector3 scale = spriteTransform.localScale * coefSize;
                spriteTransform.localScale = scale;

                // JUMP
                CameraSwitcher.Instance.SwitchCamera();
                CameraShaker.Instance.Shake();
                
                float durationTime = endTime - startTimeHold;
                float jumpForce = GetJumpForceFromTime(durationTime);
                rb2D.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);

                currentState = EState.Jump;
            }
        }
        
        private void Shrink(float startTime)
        {
            if (currentState == EState.Normal && isOnGround)
            {
                currentState = EState.Shrink;
                
                if (_jumpChargeUI == null)
                {
                    _jumpChargeUI = UIManager.Instance.gameObject.GetComponent<JumpChargeUI>();
                    _jumpChargeUI.InitChargeSlider(maxHoldTime);
                }
                
                startTimeHold = startTime;
                CameraSwitcher.Instance.SwitchCamera();
                
                holdDuration = 0f;
                Vector3 scale = spriteTransform.localScale / coefSize;
                spriteTransform.localScale = scale;
            }
        }

        
        private void Dive()
        {
            // DIVE
            currentState = EState.Dive;
            Vector2 diveImpulse = new Vector2(0f,-1 * diveForce);
            rb2D.AddForce(diveImpulse, ForceMode2D.Impulse);
        }

        private void JumpBack()
        {
            if (currentState == EState.Bounce && CanTap)
            {
                HasTap = true;
                float velocityAbs = Mathf.Abs(previousVelocity.y);
                //float minForce = Mathf.Max(velocityAbs, bounceForceMin);
                //Vector2 bounceForce = normal * (minForce + addBounceForce);
                Vector2 bounceForce = normal * velocityAbs;
                Debug.Log("bounce force : "+bounceForce);
                rb2D.AddForce(bounceForce, ForceMode2D.Impulse);
                StopCoroutine(ReactionTimerCoroutine());
            }
        }

        private float GetJumpForceFromTime(float t)
        {
            int length = jumpSteps.Length;
            
            for (int i = 0; i < length; i++)
            {
                if (i == length - 1) return jumpSteps[i].y;
                if (t >= jumpSteps[i].x && t < jumpSteps[i + 1].x) return jumpSteps[i].y;
            }

            return jumpSteps[0].y;
        }
        
        private void CheckGround()
        {
            RaycastHit2D hit2d = Physics2D.Raycast(transform.position, Vector2.down, checkFloorDist, groundMask);

            if (hit2d.collider != null)
            {
                isOnGround = true;
            }
            else
            {
                isOnGround = false;
            }
        }

        private void CheckForTapInput(Vector2 direction)
        {
            RaycastHit2D hit2d = Physics2D.Raycast(transform.position, direction, distanceFromGroundToTap, groundMask);
            Debug.DrawRay(transform.position, direction * distanceFromGroundToTap, Color.yellow, 0.2f);
            
            if (hit2d.collider != null)
            {
                CanTap = true;
            }
            else
            {
                CanTap = false;
            }
        }
        
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            
            if (currentState == EState.Normal || currentState == EState.Shrink)
                return;
            
            // 6 : Ground Layer
            if (other.gameObject.layer == 6)
            {
                ContactPoint2D contact = other.contacts[0];
                normal = contact.normal;
                normal = normal.y > 0f ? Vector2.up : Vector2.down;
                Debug.DrawRay(contact.point, contact.normal, Color.cyan, 0.5f);

                if (currentState == EState.Jump)
                {
                    if (normal == Vector2.down)
                        currentState = EState.Bounce;
                    else
                        currentState = EState.Normal;
                }
                
                if (currentState == EState.Bounce)
                {
                    StartCoroutine(ReactionTimerCoroutine());
                }
            }
        }

        private IEnumerator ReactionTimerCoroutine()
        {
            CanTap = true;
            yield return new WaitForSeconds(reactionTime);
            CanTap = false;
            
            if (!HasTap)
            {
                currentState = EState.Normal;
            }
            else
            {
                HasTap = false;
            }
        }

        private void OnDrawGizmos()
        {
            if (CanTap)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(transform.position,0.5f);
            }

            if (HasTap)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(transform.position,0.5f);
            }
        }
    }
}
