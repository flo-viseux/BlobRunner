using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Player
{
    public class JumpState : IPlayerState
    {
        private JumpSpec jumpSpec = null;
        private float jumpStartTime;
        
        public void OnEnterState(PlayerController playerController)
        {
            Debug.Log("Jump State");
            
            if (jumpSpec == null)
                jumpSpec = playerController.jumpSpec;
            
            jumpStartTime = Time.time;
            playerController.rb2D.velocity = new Vector3(0, jumpSpec.initialJumpForce, 0);
            
            // sound jump
        }

        public void LogicUpdate(PlayerController playerController, float deltaTime)
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
        }

        public void PhysicsUpdate(PlayerController playerController, float fixedDeltaTime)
        {

        }

        public void OnExitState(PlayerController playerController)
        {
            
        }
    }
}
