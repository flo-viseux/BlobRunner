using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Player
{
    public class PlayerController : MonoBehaviour
    {
        public Rigidbody2D rb2D;
        public float gravityFall = 5f;

        [Header("Input")]
        [SerializeField] private InputManager inputManager;
        
        [Header("Jump State")]
        public JumpSpec jumpSpec;

        [Header("Bounce State")] 
        public PhysicsMaterial2D physicsMaterial2D;
        public JumpSpec bounceSpec;
        public JumpSpec bounceChemicalSpec;
        public float addSpeed;
        public float delayBetweenAddSpeed;
        public float minBounceVelocity = 2f;
        public float bounceMultiplier = 1.5f;
        
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
        [SerializeField] private string plutoniumTag = "Plutonium";
        [SerializeField] private int plutoniumPoint = 10;
        public LayerMask groundMask;
        public LayerMask obstaclesMask;

        private bool isOnGround;
        public bool IsOnGround() => isOnGround;

        [HideInInspector] public bool isBouncingFromChemical;
        
        public PlayerStateMachine stateMachine;
        [HideInInspector] public float startGravity;

        private void Start()
        {
            startGravity = rb2D.gravityScale;
            stateMachine = new PlayerStateMachine(this, inputManager);
            stateMachine.SubscribeToInput();

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
                //Debug.Log("is on ground "+ hit2d.collider.name);
            }
            else
            {
                //Debug.Log("is in air ");
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
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(chemicalXTag))
            {
                isBouncingFromChemical = true;
            }
            else if (other.gameObject.CompareTag(plutoniumTag))
            {
                GameManager.Instance.playerDatas.AddScore(plutoniumPoint);
            }
        }

        private void OnDestroy()
        {
            stateMachine.UnsubscribeToInput();
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
