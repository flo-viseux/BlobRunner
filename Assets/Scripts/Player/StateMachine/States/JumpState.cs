using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Player
{
    public class JumpState : IPlayerState
    {
        private float durationTime;
        public void SetDurationTime(float value) => durationTime = value;

        private PlayerController controller;
        private float jumpForceMultiplier;
        public bool isTap;

        private Vector2 normal;
        private bool isBouncing;
        
        public void OnEnterState(PlayerController playerController)
        {
            Debug.Log("Jump State");
            if (controller == null)
                controller = playerController;
            
            // TODO :sound jump
            
            playerController.OnHitGround += GoToBounce;
            
            if (!isTap)
            {
                durationTime = Mathf.Clamp(durationTime, 0, playerController.maxHoldTime);
                jumpForceMultiplier = Mathf.Lerp(1f, playerController.maxJumpForceMultiplier,
                    durationTime / playerController.maxHoldTime);
            }
            else
            {
                jumpForceMultiplier = 1f;
            }
            Vector2 jumpImpulse = new Vector2(0f, playerController.jumpForce * jumpForceMultiplier);
            playerController.rb2D.AddForce(jumpImpulse, ForceMode2D.Impulse);
        }

        public void LogicUpdate(PlayerController playerController, float deltaTime)
        {
 
        }

        public void PhysicsUpdate(PlayerController playerController, float fixedDeltaTime)
        {
            
        }

        public void OnExitState(PlayerController playerController)
        {
            playerController.OnHitGround -= GoToBounce;
            if (isBouncing)
            {
                normal = normal.y > 0f ? Vector2.up : Vector2.down;
                Vector2 bounceForce = playerController.bounceForce * normal;
                playerController.rb2D.AddForce(bounceForce, ForceMode2D.Impulse);
            }
        }

        private void GoToBounce(Vector2 newDirection)
        {
            if (newDirection.y < 0f)
            {
                isBouncing = true;
                normal = newDirection;
                //controller.stateMachine.Bounce();
                PlayerController.stateMachine.Bounce();
            }
            else
            {
                isBouncing = false;
                //controller.stateMachine.NormalState();
                PlayerController.stateMachine.NormalState();
            }
        }
    }
}
