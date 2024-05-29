using System.Collections;
using System.Collections.Generic;
using Runner.Player;
using UnityEngine;
using UnityEngine.Rendering;

namespace Runner.Player
{
    public class BounceState : IPlayerState
    {
        private float durationTime;
        public void SetDurationTime(float value) => durationTime = value;
        
        private float timer = 0f;
        private float bounceForce;
        private JumpSpec bounceSpec;

        private float gravityBeforeBounce;
        private float currentVelocity;
        private Vector2 direction;
        
        public void OnEnterState(PlayerController playerController)
        {
            Debug.Log("Bounce state");
            
            
            timer = 0f;
            /*
            if (playerController.isBouncingFromChemical)
            {
                bounceSpec = playerController.bounceChemicalSpec;
            }
            else
            {
                bounceSpec = playerController.bounceSpec;
            }

            bounceForce = bounceSpec.gravityRise.Evaluate(durationTime);
            */
            // gravityBeforeBounce = playerController.rb2D.gravityScale;
            // //playerController.rb2D.velocity = new Vector2(0f, playerController.minBounceVelocity);
            // currentVelocity = playerController.minBounceVelocity;
            // playerController.rb2D.gravityScale = 10f;
            // direction = Vector2.up;
            playerController.rb2D.sharedMaterial = playerController.physicsMaterial2D;
            playerController.rb2D.AddForce(playerController.minBounceVelocity*Vector2.up,ForceMode2D.Impulse);
        }

        public void LogicUpdate(PlayerController playerController, float deltaTime)
        {
            // TODO : handle sounds
            
            
            // Increase Speed
            timer += deltaTime;
            if (timer >= playerController.delayBetweenAddSpeed)
            {
                timer = 0;
                GameManager.Instance.playerDatas.IncreaseSpeed(playerController.addSpeed);
            }
        }

        public void PhysicsUpdate(PlayerController playerController, float fixedDeltaTime)
        {
            
            // Rigidbody2D rb = playerController.rb2D;
            // Vector2 currentVelocity = rb.velocity;
            // float verticalDistance = playerController.checkForwardDistNormal;
            //
            // // Collision with ceiling
            // RaycastHit2D hitUp = Physics2D.Raycast(rb.position, Vector2.up, verticalDistance, playerController.groundMask);
            // if (hitUp.collider != null && hitUp.normal.y < -0.9f)
            // {
            //     direction = Vector2.down;
            //     currentVelocity.y *= playerController.bounceMultiplier;
            //     // rb.velocity = currentVelocity;
            //     rb.AddForce(direction * currentVelocity, ForceMode2D.Impulse);
            // }
            //
            // // Collision with floor
            // RaycastHit2D hitDown = Physics2D.Raycast(rb.position, Vector2.down, verticalDistance, playerController.groundMask);
            // if (hitDown.collider != null && hitDown.normal.y < 0.9f)
            // {
            //     direction = Vector2.up;
            //     currentVelocity.y *= playerController.bounceMultiplier;
            //     // rb.velocity = currentVelocity;
            //     rb.AddForce(direction * currentVelocity, ForceMode2D.Impulse);
            // } 

            if (playerController.IsOnGround() && playerController.rb2D.velocity.y == 0f)
            {
                playerController.stateMachine.NormalState();
            }
        }


        public void OnExitState(PlayerController playerController)
        {
            GameManager.Instance.playerDatas.ResetSpeed();
            // playerController.rb2D.velocity = Vector2.zero;
            // playerController.rb2D.gravityScale = gravityBeforeBounce;
            playerController.rb2D.sharedMaterial = null;
        }
        
    }
}
