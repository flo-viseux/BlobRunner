using System.Collections;
using System.Collections.Generic;
using Runner.Player;
using UnityEngine;
using UnityEngine.Rendering;

namespace Runner.Player
{
    public class BounceState : IPlayerState
    {
        private float timer = 0f;
        private Vector2 previousVelocity;
        private Vector2 direction;
        private float multiplier;
        
        private float bounceForce;
        private float maxForce;

        private bool firstImpulse = false;
        private int countBounce = 0;

        private bool isAddingSpeedWithBouncingTime;
        
        // previous velocity (fixed update)
        // relative velocity (collider2D)
        // other script to handle OnCollisionEnter et s'abonner ici
        
        public void OnEnterState(PlayerController playerController)
        {
            Debug.Log("Bounce state");
            playerController.OnHitGround += SwitchDirection;

            bounceForce = playerController.bounceForce;
            maxForce = playerController.maxBounceForce;
            multiplier = playerController.bounceMultiplier;
            isAddingSpeedWithBouncingTime = playerController.addSpeedWithTime;
            
            timer = 0f;
            countBounce = 0;
            direction = Vector2.up;
            previousVelocity = direction * 10f;
            
            playerController.rb2D.AddForce(direction * bounceForce, ForceMode2D.Impulse);
            firstImpulse = true;
        }

        public void LogicUpdate(PlayerController playerController, float deltaTime)
        {
            // TODO : handle sounds

            // TODO : GD & LD need to decide if speed evolve with time or bounce count
            if (isAddingSpeedWithBouncingTime)
            {
                timer += deltaTime;
                if (timer >= playerController.delayBetweenAddSpeed)
                {
                    timer = 0;
                    GameManager.Instance.playerDatas.IncreaseSpeed(playerController.addSpeed);
                }
            }
            else
            {
                if (countBounce % playerController.addSpeedBetweenNBounce == 0)
                {
                    GameManager.Instance.playerDatas.IncreaseSpeed(playerController.addSpeed);
                }
            }
        }

        public void PhysicsUpdate(PlayerController playerController, float fixedDeltaTime)
        {
            if (firstImpulse)
            {
                if (playerController.rb2D.velocity.y < bounceForce) return;
                else firstImpulse = false;
            }
            
            playerController.rb2D.velocity = previousVelocity;
        }

        private void SwitchDirection(Vector2 newDirection)
        {
            // TODO : delete if speed evolve with time
            if (!isAddingSpeedWithBouncingTime) countBounce++;
            
            if (newDirection.y > 0f) direction = Vector2.up;
            else direction = Vector2.down;

            previousVelocity = Mathf.Min(Mathf.Abs(previousVelocity.y) + multiplier,maxForce) * direction;
        }


        public void OnExitState(PlayerController playerController)
        {
            GameManager.Instance.playerDatas.ResetSpeed();
            playerController.OnHitGround -= SwitchDirection;
        }
        
    }
}
