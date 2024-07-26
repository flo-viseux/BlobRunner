using System;
using System.Collections;
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
        [SerializeField] private Player3DAnimator animator;
        
        // [Header("Shrink")] 
        // [SerializeField] private Transform _spriteTransform;
        // [SerializeField] private float  shrinkSize;
        // [SerializeField]private float coefSize = 2f;

        [Header("Jump")] 
        [SerializeField] private float gravity = 5f;
        [SerializeField] private Vector2[] jumpSteps;
        
        [Header("Dive")] 
        [SerializeField] private float diveForce = 1f;

        [Header("Events")] 
        [SerializeField] private VFX_EventSO VFX_Event;
        [SerializeField] private SFX_EventSO SFX_Event;
        [SerializeField] private SliderEventSO sliderEvent;

        
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
        
        // #region Events
        // // animator
        // public event Action Jumped;
        // public event Action Run;
        // public event Action<bool, float> GroundChange;
        // #endregion

        private Vector3 shrinkScaleTarget;
        
        private void Awake()
        {
            _input = GetComponent<InputManager>();
        }

        private void Start()
        {
            // originalScale = _spriteTransform.localScale;
            // shrinkScaleTarget = originalScale / coefSize;
            
            transformX = transform.position.x;
            isGrounded = false;
            
            e_jumpType = EJumpType.None;
            allowJump = false;
            sliderEvent.RaiseInitEvent(jumpSteps[2].x);
            
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
            groundComp.OnHitHead += OnHitHead;
            jumpBufferComp.OnJumpBuffer += OnAllowJump;
        }

        private void OnDisable()
        {
            _input.OnStartTouch -= OnStartShrink;
            _input.OnHold -= HoldShrink;
            _input.OnEndTouch -= OnEndShrink;
            _input.OnTap -= OnJump;
            
            groundComp.OnGround -= OnGround;
            groundComp.OnHitHead -= OnHitHead;
            jumpBufferComp.OnJumpBuffer -= OnAllowJump;
        }

        private void OnHitHead(bool hasHit, bool hasGround)
        {
            if (!hasHit) return;
            
            if (hasGround)
            {
                velocity = 0f;
                return;
            }
            
            if (!isGrounded && velocity > 0)
            {
                EventManager.RaiseHitHeadEvent();
                velocity = -velocity;
            }
        }
        private void OnAllowJump(bool value)
        {
            allowJump = value;
        }

        private void OnGround(bool isOnGround, Collider2D p_collider)
        {
            if (p_collider == null)
            {
                isGrounded = false;
                animator.FallAnimation();
            }
            else
            {
                isGrounded = true;
                EventManager.RaiseHitGroundEvent();
                
                // // Play SFX
                // if (_currentState == EState.Jump)
                // {
                //     SFX_Event.RaiseJumpEvent(AudioManager.EType.HitGround);
                // }
                // else if (_currentState == EState.Dive)
                // {
                //     SFX_Event.RaiseDiveEvent(AudioManager.ETypeDive.HitGround);
                // }
            }    
            
            if (isOnGround)
            {
                float surface = p_collider.bounds.max.y;
                transform.position = new Vector2(transformX, surface);
                
                animator.LandingAnimation();
                
                if (hasTap)
                {
                    HandleHitGround();
                }
                else
                {
                    StartCoroutine(WaitOnGround());
                }
            }
        }

        
        private IEnumerator WaitOnGround()
        {
            yield return new WaitForSeconds(0.1f);
            HandleHitGround();
        }

        private void HandleHitGround()
        {
            if (_currentState == EState.Jump || _currentState == EState.Dive)
            {
                if (hasTap && allowJump)
                {
                    if (_currentState == EState.Jump)
                    {
                        SetJumpType();
                    }
                    else
                    {
                        _currentState = EState.Jump;
                    }
                    
                    
                    int index = (int)e_jumpType;
                    Jump(jumpSteps[index].y);
                    
                    EventManager.RaiseBounceEvent(transform.position);
                    
                    VFX_Event.RaiseEvent(transform.position, VFX_Manager.EType.Bounce);
                }
                else 
                {
                    _currentState = EState.Normal;
                    e_jumpType = EJumpType.None;
                    velocity = 0f;
                    
                    EventManager.RaiseRunEvent();
                }
                allowJump = false;
                hasTap = false;
            }
            //
            // // animation
            // GroundChange?.Invoke(true, 0f);
        }

        #region Shrink

        private Coroutine scaleCoroutine = null;
        
        private void OnStartShrink(float time)
        {
            if (_currentState == EState.Normal && isGrounded)
            {
                _startHoldTime = time;
                _currentState = EState.Shrink;
                //if (scaleCoroutine == null) 
                    //scaleCoroutine = StartCoroutine(ResizeCoroutine(originalScale, shrinkScaleTarget, jumpSteps[0].x));
                EventManager.RaiseShrinkEvent();
                animator.ShrinkAnimation();
            }
        }

        private void HoldShrink(float time)
        {
            if (_currentState == EState.Shrink)
            {
                _currentHoldTime = time - _startHoldTime;

                sliderEvent.RaiseUpdateEvent(_currentHoldTime);
                
                int jumpHeight = GetJumpIndexFromTime(_currentHoldTime);

                switch (jumpHeight)
                {
                    case 0:
                        CameraSwitcher.Instance.SwitchCamera(CameraSwitcher.CameraState.Default);
                        break;
                    case 1:
                        CameraSwitcher.Instance.SwitchCamera(CameraSwitcher.CameraState.Medium);
                        break;
                    case 2:
                        CameraSwitcher.Instance.SwitchCamera(CameraSwitcher.CameraState.Large);
                        break;
                }
            }
        }

        private void OnEndShrink(float time)
        {
            if (_currentState == EState.Shrink)
            {
                EventManager.RaiseEndShrinkEvent();
                animator.EndShrinkAnimation();
                
                // if (scaleCoroutine != null)
                // {
                //     StopCoroutine(scaleCoroutine);
                //     scaleCoroutine = null;
                // }
                // StartCoroutine(ResizeCoroutine(_spriteTransform.localScale, originalScale, 0.3f));
                
                _currentState = EState.Jump;
                _durationHoldTime = time - _startHoldTime;
                
                sliderEvent.RaiseResetEvent();
                
                float jumpHeight = GetJumpHeight(_durationHoldTime);
                Jump(jumpHeight);
            }
        }
        
        // private IEnumerator ResizeCoroutine(Vector3 startSize, Vector3 endSize, float duration)
        // {
        //     float time = 0f;
        //
        //     while (time < duration)
        //     {
        //         _spriteTransform.localScale = Vector3.Lerp(startSize, endSize, time / duration);
        //         time += Time.deltaTime;
        //         yield return null;
        //     }
        //     _spriteTransform.localScale = endSize;
        // }
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
                    EventManager.RaiseJumpEvent(transform.position);
                    return;
                }
            }

            hasTap = true;

            if (_currentState == EState.Jump)
            {
                if (!allowJump && !isGrounded)
                {
                    _currentState = EState.Dive;
                    
                    EventManager.RaiseDiveEvent(transform.position);
                    animator.DiveAnimation();
                    
                    velocity = diveForce * _GRAVITY;
                    hasTap = false;
                    
                    VFX_Event.RaiseEvent(transform.position, VFX_Manager.EType.Dive);
                    //SFX_Event.RaiseDiveEvent(AudioManager.ETypeDive.Dive);

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
            animator.JumpAnimation();
            //StartCoroutine(TimerLaunchAnimJump(0.2f));
            
            //SFX_Event.RaiseJumpEvent(AudioManager.EType.Small);
            
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

        // #region Timers for animations
        // private IEnumerator TimerLaunchAnimJump(float seconds)
        // {
        //     GroundChange?.Invoke(false, velocity);
        //     yield return new WaitForSeconds(seconds);
        //     Jumped?.Invoke();
        //     groundComp.ChangeWasOnGround(false);
        // }
        //
        // private IEnumerator TimerRun(float seconds)
        // {
        //     yield return new WaitForSeconds(seconds);
        //     Run?.Invoke();
        // }
        // #endregion
        
    }
}