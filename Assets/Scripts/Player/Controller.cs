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
        public float JumpHeight = 5.0f;  // Hauteur du saut du joueur
        public float jumpStartY = 0f;
        private float jumpTime;  // Temps de saut total
        private float jumpProgress = 0.0f;  // Progression du saut
        /*[Tooltip("x : time, y : jumpLength")]
        [SerializeField] private Vector2[] jumpSteps = new Vector2[3];*/

        [Tooltip("trajectoire de saut en fonction de la distance parcourue")]
        [SerializeField] private AnimationCurve jumpCurve;
        [SerializeField] private AnimationCurve jumpTrajectoryCurve;
        [SerializeField] private float maxJumpLength = 5f;

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
        private bool isJumping;
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
            /*int lastIndex = jumpSteps.Length - 1;
            maxHoldTime = jumpSteps[lastIndex].x;*/
            HasTap = false;
            CanTap = false;

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
            // debug
            UpdateColor();
            if (UIDebugText.Instance != null)
                UIDebugText.Instance.UpdateText(currentState.ToString());
            //----
            
            CheckGround();
            
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

        private void Jump() //(float endTime)
        {
            /*if (currentState == EState.Shrink)
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
            }
            else if (currentState == EState.Normal)
            {
                float jumpForce = GetJumpForceFromTime(0);
                rb2D.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            }*/

            if (!isOnGround || isJumping)
                return;

            StartCoroutine(JumpTrajectory(SectionGenerator.Instance.CurrentPos));
            currentState = EState.Jump;
        }

        private void Jump(float endTime)
        {
            if (!isOnGround || isJumping)
                return;

            StartCoroutine(JumpTrajectory(SectionGenerator.Instance.CurrentPos));
            currentState = EState.Jump;
        }

        private IEnumerator JumpTrajectory(float startJumpPos)
        {
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
                Debug.Log(newY);

                // Mettre à jour la position du joueur
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);

                // Vérifier si le saut est terminé
                if (jumpProgress >= maxJumpLength)
                {
                    isJumping = false;
                    jumpProgress = 0.0f;
                    transform.position = new Vector3(transform.position.x, jumpStartY, transform.position.z);
                }
                yield return null;
            }
        }

        private void Shrink(float startTime)
        {
            /*if (currentState == EState.Normal && isOnGround)
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
            }*/
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

        /*private float GetJumpForceFromTime(float t)
        {
            int length = jumpSteps.Length;
            
            for (int i = 0; i < length; i++)
            {
                if (i == length - 1) return jumpSteps[i].y;
                if (t >= jumpSteps[i].x && t < jumpSteps[i + 1].x) return jumpSteps[i].y;
            }

            return jumpSteps[0].y;
        }

        private int GetJumpTypeFromTime(float t)
        {
            int length = jumpSteps.Length;

            for (int i = 0; i < length; i++)
            {
                if (i == length - 1) return i;
                if (t >= jumpSteps[i].x && t < jumpSteps[i + 1].x) return i;
            }

            return 0;
        }
*/
        private void CheckGround()
        {
            RaycastHit2D hit2d = Physics2D.Raycast(transform.position, Vector2.down, checkFloorDist, groundMask);

            if (hit2d.collider != null)
            {
                /*if (isJumping)
                    isJumping = false;*/

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

            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, Vector2.down * checkFloorDist);
        }
    }
}
