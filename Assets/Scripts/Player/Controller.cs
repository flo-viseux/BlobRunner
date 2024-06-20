using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.HID;

namespace Runner.Player
{
    public enum EState
    {
        Normal,
        Shrink,
        Jump,
        Bounce,
        Dive,
    }

    public class Controller : MonoBehaviour
    {
        public static EState currentState;

        [Header("Debug")]
        [Tooltip("Normal, Shrink, Bounce")]
        public Color[] stateColor;
        public SpriteRenderer _renderer;
        
        [Header("Components")]
        [SerializeField] private InputManager inputManager;
        [SerializeField] private Rigidbody2D rb2D;
        [SerializeField] private Transform spriteTransform;
        
        [Header("Ground")]
        [SerializeField] private float checkFloorDist;
        [SerializeField] private float checkFloorJumpAgainDist;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float initialJumpAgainCd = .5f;

        [Header("Jump")]
        [Tooltip("x : time, y : jumpHeight, z : jumpLength")]
        [SerializeField] private Vector3[] jumpSteps = new Vector3[3];

        /*[Header("Bounce")] 
        [SerializeField] private float bounceForceMin = 15f;
        [SerializeField] private float distanceFromGroundToTap = 0.2f;
        [SerializeField] private float addBounceForce = 0.1f;
        [SerializeField] private float reactionTime = 0.5f;*/
        
        [Header("Shrink")]
        [SerializeField] private float coefSize;
        
        [Header("Dive")]
        [SerializeField] private float diveForce;

        #region Attributes
        // hold
        private float startTimeHold;
        private float holdDuration;
        private float maxHoldTime;

        // ground
        private bool isOnGround;

        // jump
        private bool isJumping; 
        private float jumpAgainCd;
        private float JumpHeight = 5.0f;  // Hauteur du saut du joueur
        public float jumpStartY = 0f;
        private float jumpTime;  // Temps de saut total
        private float jumpProgress = 0.0f;  // Progression du saut
        private bool WillJumpAgain = false;
        private int jumpStepIndex = 0;
        private float maxJumpLength = 5f;
        #endregion

        #region Delegates
        public delegate void OnDiveStart(Vector3 playerPos);
        public OnDiveStart onDiveStart;

        public delegate void OnBounce(Vector3 playerPos);
        public OnBounce onBounce;
        #endregion

        // UI : slider 
        private JumpChargeUI _jumpChargeUI = null;
        public JumpChargeUI GetJumpChargeUI() => _jumpChargeUI;

        #region UnityMethods
        private void Start()
        {
            currentState = EState.Normal;
            /*int lastIndex = jumpSteps.Length - 1;
            maxHoldTime = jumpSteps[lastIndex].x;
            HasTap = false;
            CanTap = false;*/
            WillJumpAgain = false;
            jumpStartY = transform.position.y;
        }

        private void OnEnable()
        {
            inputManager.OnStartTouch += Shrink;
            inputManager.OnEndTouch += Jump;
            inputManager.OnSwipeSuccessful += Dive;
            inputManager.OnTap += Jump;
        }

        private void OnDisable()
        {
            inputManager.OnStartTouch -= Shrink;
            inputManager.OnEndTouch -= Jump;
            inputManager.OnSwipeSuccessful -= Dive;
            inputManager.OnTap -= Jump;
        }

        private void Update()
        {
            jumpAgainCd -= Time.deltaTime;

            // debug
            UpdateColor();
            if (UIDebugText.Instance != null)
                UIDebugText.Instance.UpdateText(currentState.ToString());
            //----

            // debug, charge ui
            if (currentState == EState.Shrink)
            {
                if (holdDuration > maxHoldTime) return;
                holdDuration += Time.deltaTime;
                GetJumpChargeUI().UpdateCharge(holdDuration);
                return;
            }

            // fait repasser en état normal après un dive
            if (currentState == EState.Dive && isOnGround)
            {
                currentState = EState.Normal;
                return;
            }
        }
        private void FixedUpdate()
        {
            CheckGround();
            /* if (CanTap) return;
             previousVelocity = rb2D.velocity;*/
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (currentState == EState.Normal || currentState == EState.Shrink)
                return;

            if (other.gameObject.layer == 6)
            {
                isOnGround = true;
                isJumping = false;
                jumpAgainCd = initialJumpAgainCd;
                currentState = EState.Normal;
                StopCoroutine(JumpTrajectory());
                Debug.Log("collision - ground no normal state");
            }
        }

        private void OnDrawGizmos()
        {
            /*if (CanTap)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(transform.position, 0.5f);
            }

            if (HasTap)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(transform.position, 0.5f);
            }*/

            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, Vector2.down * checkFloorDist);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector2.down * checkFloorJumpAgainDist);
        }
        #endregion


        #region Private
        private void Shrink(float startTime)
        {
            if (currentState == EState.Normal && isOnGround)
            {
                Debug.Log("Shrink");

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


        #region Jump
        private void CheckGround()
        {
            RaycastHit2D hit2d = Physics2D.Raycast(transform.position, Vector2.down, checkFloorDist, groundMask);

            if (hit2d.collider != null)
            {
                if (isJumping)
                {
                    Debug.Log("ground");
                    jumpAgainCd = initialJumpAgainCd;
                    currentState = EState.Normal;
                    isJumping = false;
                    StopCoroutine(JumpTrajectory());
                }

                /*if (WillJumpAgain)
                {
                    jumpStepIndex = Mathf.Min(jumpStepIndex + 1, jumpSteps.Length - 1);

                    Debug.Log("WillJumpAgain, " + jumpStepIndex);
                    StartCoroutine(JumpTrajectory());
                    return;
                }*/

                //jumpStepIndex = 0;
                isOnGround = true;
            }
            else
            {
                isOnGround = false;
            }
        }
        private void Jump()
        {
        }
        private void Jump(float endTime)
        {
            //Debug.Log("Jump");

            if (!isOnGround || isJumping)
            {
                WillJumpAgain = IsWillJumpAgain();
                return;
            }

            if (jumpAgainCd > 0f || WillJumpAgain)
            {
                jumpStepIndex = Mathf.Min(jumpStepIndex + 1, jumpSteps.Length - 1);

                Debug.Log("JumpAgain, " + jumpStepIndex);
            }
            else
            {
                jumpStepIndex = 0;
            }
            
            JumpHeight = jumpSteps[jumpStepIndex].y;
            maxJumpLength = jumpSteps[jumpStepIndex].z;

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
                jumpStepIndex = GetJumpIndexFromTime(durationTime);
                JumpHeight = jumpSteps[jumpStepIndex].y;
                maxJumpLength = jumpSteps[jumpStepIndex].z;
            }
            else if (jumpStepIndex > 0)
            {
                if (onBounce != null)
                    onBounce(transform.position);

                JumpHeight = jumpSteps[jumpStepIndex].y;
                maxJumpLength = jumpSteps[jumpStepIndex].z;
            }

            StartCoroutine(JumpTrajectory());
        }
        private IEnumerator JumpTrajectory()
        {
            WillJumpAgain = false;
            currentState = EState.Jump;
            int t = 0;
            isJumping = true;
            isOnGround = false;
            jumpProgress = 0;

            while (isJumping)
            {
                // Calculer la progression du saut en fonction de la distance parcourue
                jumpProgress += SectionGenerator.Instance.Speed * Time.deltaTime;

                // Calculer la nouvelle position en Y du joueur en fonction de la hauteur de saut théorique
                float newY = jumpStartY + JumpHeight * (Mathf.Sin(Mathf.PI * (jumpProgress / maxJumpLength)));

                // Mettre à jour la position du joueur
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);

                // Vérifier si le saut est terminé
                if (jumpProgress >= maxJumpLength)
                {
                    Debug.Log("EndJump");
                    isJumping = false;
                    jumpAgainCd = initialJumpAgainCd;
                    jumpProgress = 0.0f;
                    rb2D.velocity = new Vector3(0, -5, 0);
                }
                yield return null;
            }
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
        private bool IsWillJumpAgain()
        {
            RaycastHit2D hit2d = Physics2D.Raycast(transform.position, Vector2.down, checkFloorJumpAgainDist, groundMask);
            return (hit2d.collider != null);
        }
        #endregion


        // debug only
        private void UpdateColor()
        {
            switch (currentState)
            {
                case EState.Normal:
                    _renderer.color = stateColor[0];
                    return;
                case EState.Shrink:
                    _renderer.color = stateColor[1];
                    return;
                case EState.Bounce:
                    _renderer.color = stateColor[2];
                    return;
                default:
                    _renderer.color = Color.white;
                    return;
            }
        }

        private void Dive()
        {
            // DIVE
            if (onDiveStart != null)
                onDiveStart(transform.position);

            currentState = EState.Dive;
            isJumping = false;
            Vector2 diveImpulse = new Vector2(0f,-1 * diveForce);
            rb2D.AddForce(diveImpulse, ForceMode2D.Impulse);
        }
        #endregion
    }
}
