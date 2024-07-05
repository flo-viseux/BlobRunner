using System;
using System.Collections;
using TMPro.EditorUtilities;
using TMPro.Examples;
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
        public enum EJumpType
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
        [SerializeField] private float  shrinkSize;
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
        private float _currentHoldTime;
        private float _durationHoldTime;
        private Vector3 originalScale;
        
        // Jump
        private float _GRAVITY;
        private EJumpType e_jumpType;
        private bool hasTap;
        private bool allowJump;

        public EJumpType GetJumpType() => e_jumpType;
        
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
            _input.OnHold += HoldShrink;
            _input.OnEndTouch += OnEndShrink;
            _input.OnTap += OnJump;

            groundComp.OnGround += OnGround;
            jumpBufferComp.OnJumpBuffer += OnAllowJump;
        }

        private void OnDisable()
        {
            _input.OnStartTouch -= OnStartShrink;
            _input.OnHold -= HoldShrink;
            _input.OnEndTouch -= OnEndShrink;
            _input.OnTap -= OnJump;
            
            groundComp.OnGround -= OnGround;
            jumpBufferComp.OnJumpBuffer -= OnAllowJump;
        }

        private void OnAllowJump(bool value)
        {
            allowJump = value;
        }

        private void OnGround(bool isOnGround, Collider2D p_collider)
        {
            isGrounded = isOnGround;
            
            if (isOnGround)
            {
                surface = Physics2D.ClosestPoint(transform.position, p_collider);
                transform.position = new Vector2(transformX, surface.y);
                
                StartCoroutine(WaitOnGround(p_collider));
            }
        }

        
        private IEnumerator WaitOnGround(Collider2D p_collider)
        {
            yield return new WaitForSeconds(0.1f);
            
            if (_currentState == EState.Jump || _currentState == EState.Dive)
            {
                if (hasTap && allowJump)
                {
                    if (_currentState == EState.Jump) SetJumpType();
                    else _currentState = EState.Jump;
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
        }



        #region Shrink
        
        private void OnStartShrink(float time)
        {
            if (_currentState == EState.Normal && isGrounded)
            {
                _startHoldTime = time;
                _currentState = EState.Shrink;
            }
        }

        private void HoldShrink(float time)
        {
            if (_currentState == EState.Shrink)
            {
                _currentHoldTime = time - _startHoldTime;
                int jumpHeight = GetJumpIndexFromTime(_currentHoldTime);

                switch (jumpHeight)
                {
                    case 0:
                        Debug.Log("Default");
                        CameraSwitcher.Instance.SwitchCamera(CameraSwitcher.CameraState.Default);
                        break;
                    case 1:
                        Debug.Log("Medium");
                        _spriteTransform.localScale = originalScale / coefSize;
                        CameraSwitcher.Instance.SwitchCamera(CameraSwitcher.CameraState.Medium);
                        break;
                    case 2:
                        Debug.Log("Large");
                        CameraSwitcher.Instance.SwitchCamera(CameraSwitcher.CameraState.Large);
                        break;
                }
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
                    Debug.Log($"dive - is grounded {isGrounded}");
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
            CameraSwitcher.Instance.SwitchCamera(CameraSwitcher.CameraState.Default);
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
                if (i == length - 1)
                    return i;
                if (t >= jumpSteps[i].x && t < jumpSteps[i + 1].x)
                    return i;
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