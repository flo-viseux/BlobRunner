using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Player
{
    public class PlayerController : MonoBehaviour
    {
        public Rigidbody2D rb2D;
        
        [Header("Input")]
        [SerializeField] private InputManager inputManager;

        [Header("Jump State")] 
        public float jumpForce = 10f;
        public float maxHoldTime = 2f;
        [Tooltip("Used for hold")]
        public float maxJumpForceMultiplier = 2f;

        public float windowDurationBounce = 0.2f;
            
        [Header("Bounce State : Values")] 
        public float bounceForce = 10f;
        public float maxBounceForce = 15f;
        [Tooltip("Force added to current NOT multiplied")]
        public float bounceMultiplier = 0.1f;
        [Header("Bounce State : Background Speed")]
        [Tooltip("Speed added to background")]
        public float addSpeed;
        public float delayBetweenAddSpeed;
        [Tooltip("N is the nb bounce when the speed is being added")]
        public int addSpeedBetweenNBounce;
        [Tooltip("True test with time, False test with nb bounce")]
        public bool addSpeedWithTime;
        
        [Header("Normal State")] 
        [SerializeField] private float checkCeilingDistNormal = 1f;
        public float checkForwardDistNormal = 1f;

        [Header("Shrink State")] 
        public Transform spriteTransform;
        [SerializeField] private float checkCeilingDistShrink = 0.5f;
        public float checkForwardDistShrink = 0.5f;

        [Header("Dive State")] 
        public float diveForce = 5f;
        
        [Header("")] 
        [SerializeField] private float checkFloorDist = 1f;
        [SerializeField] private string chemicalXTag = "ChemicalX";
        /*[SerializeField] private string plutoniumTag = "Plutonium";*/
        [SerializeField] private int plutoniumPoint = 10;
        public LayerMask groundMask;
        public LayerMask obstaclesMask;

        private bool isOnGround;
        public bool IsOnGround() => isOnGround;

        [HideInInspector] public bool isBouncingFromChemical;
        
        public static PlayerStateMachine stateMachine;
        [HideInInspector] public float startGravity;

        private JumpChargeUI _jumpChargeUI;
        public JumpChargeUI GetJumpChargeUI() => _jumpChargeUI;
        public event Action<Vector2> OnHitGround;

        private void Start()
        {
            startGravity = rb2D.gravityScale;
            stateMachine = new PlayerStateMachine(this, inputManager);
            stateMachine.SubscribeToInput();
            _jumpChargeUI = UIManager.Instance.gameObject.GetComponent<JumpChargeUI>();
            _jumpChargeUI.InitChargeSlider(maxHoldTime);
        }

        private void Update()
        {
            CheckGround();
            stateMachine.currentState.LogicUpdate(this, Time.deltaTime);
        }

        private void FixedUpdate()
        {
            stateMachine.currentState.PhysicsUpdate(this, Time.fixedDeltaTime);
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

        private void OnCollisionEnter2D(Collision2D other)
        {
          
            if (!isBouncingFromChemical && other.gameObject.CompareTag("Obstacles"))
            {
                GameManager.Instance.playerDatas.CurrentHealth -= 1;
                return;
            }
            
            // 6 : Ground Layer
            if (other.gameObject.layer == 6)
            {
                ContactPoint2D contact = other.contacts[0];
                OnHitGround?.Invoke(contact.normal);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(chemicalXTag))
            {
                isBouncingFromChemical = true;
            }
            /*else if (other.gameObject.CompareTag(plutoniumTag)) // Already check in collectible effect
            {
                GameManager.Instance.playerDatas.AddScore(plutoniumPoint);
            }*/
        }

        private void OnDestroy()
        {
//            stateMachine.UnsubscribeToInput();
        }
        
        void OnDrawGizmos()
        {
            Vector3 origin = transform.position;
            Vector3 direction = Vector2.down * checkFloorDist;

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(origin, direction);
            
        }
    }
}
