using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Player
{
    public class JumpState : IPlayerState
    {
        private JumpSpec jumpSpec = null;
        private float jumpStartTime;

        private bool isJumping;
        private float startGravity;
        
        public void OnEnterState(PlayerController playerController)
        {
            Debug.Log("Jump State");

            isJumping = false;

            
            if (jumpSpec == null)
                jumpSpec = playerController.jumpSpec;

            jumpStartTime = Time.time;
            playerController.rb2D.velocity = new Vector3(0, jumpSpec.initialJumpForce, 0);
            
            // TODO :sound jump
        }

        public void LogicUpdate(PlayerController playerController, float deltaTime)
        {
 
        }

        public void PhysicsUpdate(PlayerController playerController, float fixedDeltaTime)
        {
            
            float timeSinceJump = Time.time - jumpStartTime;

            if (playerController.rb2D.velocity.y > 0)
            {
                playerController.rb2D.velocity += Vector2.up * (jumpSpec.gravityRise.Evaluate(timeSinceJump) * Time.deltaTime);
            }
            else if (playerController.rb2D.velocity.y < 0)
            {
                playerController.rb2D.velocity += Vector2.up * (jumpSpec.gravityFall.Evaluate(timeSinceJump) * Time.deltaTime);
            }

            // Stop jumping when the player lands
            if (playerController.rb2D.velocity.y == 0)
            {
                playerController.stateMachine.NormalState();
            }
            
            // if (!isJumping)
            // {
            //     playerController.rb2D.AddForce(Vector2.up, ForceMode2D.Impulse);
            // }
        }

        public void OnExitState(PlayerController playerController)
        {

        }
    }
}
