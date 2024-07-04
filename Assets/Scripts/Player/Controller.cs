using System;
using System.Collections;
using Unity.VisualScripting;
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
            Dive,
        }
        private enum EJumpType
        {
            Small = 0,
            Medium,
            High,
            None
        }
        
        private InputManager _input;
        [SerializeField] private GroundCheck groundComp;
        [SerializeField] private JumpBuffer jumpBufferComp;
        
        [Header("Shrink")] 
        [SerializeField] private Transform _spriteTransform;
        [SerializeField]private float coefSize = 2f;

        [Header("Jump")] 
        [SerializeField] private float gravity = 5f;
        [SerializeField] private Vector2[] jumpSteps;
        
        [Header("Dive")] 
        [SerializeField] private float diveForce = 1f;

        
        private float transformX;
        private float velocity;
        
        // State
        private static EState _currentState = EState.Normal;
        public static EState GetCurrentState() => _currentState;
        
        // Ground
        private bool isGrounded;
        private Vector2 surface;
        
        // Shrink
        private float _startHoldTime;
        private float _durationHoldTime;
        private Vector3 originalScale;
        
        // Jump
        private float _GRAVITY;
        private EJumpType e_jumpType;
        private bool hasTap;
        private bool allowJump;
        
        #region Events
        // animator
        public event Action Jumped;
        public event Action Run;
        public event Action<bool, float> GroundChange;
        #endregion
        
        private void Awake()
        {
            _input = GetComponent<InputManager>();
        }

        private void Start()
        {
            originalScale = _spriteTransform.localScale;
            
            transformX = transform.position.x;
            isGrounded = false;
            
            e_jumpType = EJumpType.None;
            allowJump = false;
            
            _GRAVITY = Physics2D.gravity.y * gravity;
        }
        
        private void Update()
        {
            if (!isGrounded)
            {
                velocity += _GRAVITY * Time.deltaTime;
                transform.Translate(new Vector2(0,velocity)*Time.deltaTime);
            }
        }

        private void OnEnable()
        {
            _input.OnStartTouch += OnStartShrink;
            _input.OnEndTouch += OnEndShrink;
            _input.OnTap += OnJump;

            groundComp.OnGround += OnGround;
            jumpBufferComp.OnJumpBuffer += OnAllowJump;
        }

        private void OnDisable()
        {
            _input.OnStartTouch -= OnStartShrink;
            _input.OnEndTouch -= OnEndShrink;
            _input.OnTap -= OnJump;
            
            groundComp.OnGround -= OnGround;
            jumpBufferComp.OnJumpBuffer -= OnAllowJump;
        }

        private void OnAllowJump()
        {
            allowJump = true;
        }

        private void OnGround(bool isOnGround, Collider2D p_collider)
        {
            isGrounded = isOnGround;
            
            if (isOnGround)
            {
                StartCoroutine(WaitOnGround(p_collider));
            }
        }

        
        private IEnumerator WaitOnGround(Collider2D p_collider)
        {
            yield return new WaitForSeconds(0.05f);
            
            if (_currentState == EState.Jump || _currentState == EState.Dive)
            {
                if (hasTap && allowJump)
                {
                    if (_currentState == EState.Jump) SetJumpType();
                    Jump(jumpSteps[(int)e_jumpType].y);
                }
                else if (!hasTap)
                {
                    _currentState = EState.Normal;
                    e_jumpType = EJumpType.None;
                    velocity = 0f;
                    
                    // animation run
                    StartCoroutine(TimerRun(0.1f));
                }
                allowJump = false;
                hasTap = false;
            }
                
            // animation
            GroundChange?.Invoke(true, 0f);
                
            surface = Physics2D.ClosestPoint(transform.position, p_collider);
            transform.position = new Vector2(transformX, surface.y);
        }



        #region Shrink
        
        private void OnStartShrink(float time)
        {
            if (_currentState == EState.Normal && isGrounded)
            {
                _currentState = EState.Shrink;
                _startHoldTime = time;
                _spriteTransform.localScale = _spriteTransform.localScale / coefSize;
            }
        }

        private void OnEndShrink(float time)
        {
            if (_currentState == EState.Shrink)
            {
                _spriteTransform.localScale = originalScale;
                
                _currentState = EState.Jump;
                _durationHoldTime = time - _startHoldTime;
                float jumpHeight = GetJumpHeight(_durationHoldTime);
                Jump(jumpHeight);
            }
        }
        #endregion
        
        // Input Jump
        private void OnJump()
        {
            if (isGrounded)
            {
                if (_currentState == EState.Normal)
                {
                    _currentState = EState.Jump;
                    e_jumpType = EJumpType.Small;
                    
                    Jump(jumpSteps[0].y);
                    
                    return;
                }
            }

            hasTap = true;
            
            if (_currentState == EState.Jump)
            {
                if (!allowJump)
                {
                    _currentState = EState.Dive;
                    velocity = diveForce * _GRAVITY;
                    hasTap = false;
                }
            }
        }

        private void SetJumpType()
        {
            switch (e_jumpType)
            {
                case EJumpType.None:
                    e_jumpType = EJumpType.Small;
                    return;
                case EJumpType.Small:
                    e_jumpType = EJumpType.Medium;
                    return;
                default: 
                    e_jumpType = EJumpType.High; 
                    return;
            }
        }

        private void Jump(float p_jumpHeight)
        {
            StartCoroutine(TimerLaunchAnimJump(0.2f));
            isGrounded = false;
            velocity = Mathf.Sqrt(p_jumpHeight * -2 * _GRAVITY);
        }

        private float GetJumpHeight(float duration)
        {
            int index = GetJumpIndexFromTime(duration);
            e_jumpType = (EJumpType)index;
            
            return jumpSteps[index].y;
        }
        
        private int GetJumpIndexFromTime(float t)
        {
            int length = jumpSteps.Length;

            for (int i = 0; i < length; i++)
            {
                if (i == length - 1) return i;
                if (t >= jumpSteps[i].x && t < jumpSteps[i + 1].x) return i;
            }
            return 0;
        }

        #region Timers for animations
        private IEnumerator TimerLaunchAnimJump(float seconds)
        {
            GroundChange?.Invoke(false, velocity);
            yield return new WaitForSeconds(seconds);
            Jumped?.Invoke();
        }
        
        private IEnumerator TimerRun(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Run?.Invoke();
        }
        #endregion
        
    }
}