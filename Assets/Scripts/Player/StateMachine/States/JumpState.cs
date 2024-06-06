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
                Debug.Log($"jump force = {playerController.jumpForce} * {jumpForceMultiplier}");
            }
            else
            {
                jumpForceMultiplier = 1f;
                Debug.Log($"jump force = {playerController.jumpForce} * {jumpForceMultiplier}");
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
        }

        private void GoToBounce(Vector2 newDirection)
        {
            if (newDirection.y < 0f)
                controller.stateMachine.Bounce();
            else 
                controller.stateMachine.NormalState();
        }
    }
}
