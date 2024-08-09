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

        [Header("Jump")]
        [SerializeField] private float gravity = 5f;
        [SerializeField] private Vector2[] jumpSteps;

        [Header("Dive")]
        [SerializeField] private float diveForce = 1f;

        [Header("VFX transform")]
        [SerializeField] private Transform baseVFX;
        [SerializeField] private Transform centerVFX;

        [Header("Events")]
        [SerializeField] private VFX_EventSO VFX_Event;
        [SerializeField] private SFX_EventSO SFX_Event;
        [SerializeField] private SliderEventSO sliderEvent;
        [SerializeField] private SimpleEventSO hitEvent;
        [SerializeField] private SimpleEventSO winEvent;


        private float transformX;
        private float velocity;

        // State
        private static EState _currentState = EState.Normal;
        public static EState GetCurrentState() => _currentState;

        // Ground
        private bool isGrounded;

        // Shrink
        private float _startHoldTime;
        private float _currentHoldTime;
        private float _durationHoldTime;

        // Jump
        private float _GRAVITY;
        private EJumpType e_jumpType;
        private bool hasTap;
        private bool allowJump;

        public EJumpType GetJumpType() => e_jumpType;


        private void Awake()
        {
            _input = GetComponent<InputManager>();
        }

        private void Start()
        {

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
                transform.Translate(new Vector2(0, velocity) * Time.deltaTime);
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

            winEvent.OnEventRaised += OnWin;
            hitEvent.OnEventRaised += OnDeath;
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

            winEvent.OnEventRaised -= OnWin;
            hitEvent.OnEventRaised -= OnDeath;
        }

        private void OnWin()
        {
            VFX_Event.RaiseEvent(centerVFX.position, VFX_Manager.EType.Victory);
        }

        private void OnDeath()
        {
            VFX_Event.RaiseEvent(centerVFX.position, VFX_Manager.EType.Death);
            animator.DeathAnimation();
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

                    VFX_Event.RaiseEvent(baseVFX.position, VFX_Manager.EType.Bounce);
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
        }

        #region Shrink

        private bool step1 = false;
        private bool step2 = false;

        private void OnStartShrink(float time)
        {
            if (_currentState == EState.Normal && isGrounded)
            {
                _startHoldTime = time;
                _currentState = EState.Shrink;

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
                        if (step1) return;
                        CameraSwitcher.Instance.SwitchCamera(CameraSwitcher.CameraState.Medium);
                        VFX_Event.RaiseEvent(centerVFX.position, VFX_Manager.EType.JumpStep);
                        step1 = true;
                        break;
                    case 2:
                        if (step2) return;
                        CameraSwitcher.Instance.SwitchCamera(CameraSwitcher.CameraState.Large);
                        VFX_Event.RaiseEvent(centerVFX.position, VFX_Manager.EType.JumpStep, true);
                        step2 = true;
                        break;
                }
            }
        }

        private void OnEndShrink(float time)
        {
            if (_currentState == EState.Shrink)
            {
                step1 = false;
                if (step2)
                {
                    VFX_Event.RaiseStopLoop(VFX_Manager.EType.JumpStep);
                    step2 = false;
                }

                EventManager.RaiseEndShrinkEvent();
                animator.EndShrinkAnimation();

                _currentState = EState.Jump;
                _durationHoldTime = time - _startHoldTime;

                sliderEvent.RaiseResetEvent();

                float jumpHeight = GetJumpHeight(_durationHoldTime);
                Jump(jumpHeight);
                VFX_Event.RaiseEvent(baseVFX.position, VFX_Manager.EType.Jump);
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
                    EventManager.RaiseJumpEvent(transform.position);
                    VFX_Event.RaiseEvent(baseVFX.position, VFX_Manager.EType.Jump);
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

    }
}