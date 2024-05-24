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

        
        public void OnEnterState(PlayerController playerController)
        {
            Debug.Log("Bounce state");
            timer = 0f;
            
            if (playerController.isBouncingFromChemical)
            {
                bounceSpec = playerController.bounceChemicalSpec;
            }
            else
            {
                bounceSpec = playerController.bounceSpec;
            }

            bounceForce = bounceSpec.gravityRise.Evaluate(durationTime);
        }

        public void LogicUpdate(PlayerController playerController, float deltaTime)
        {
            // handle sounds
            
            // Increase Speed
            timer += deltaTime;
            if (timer >= playerController.delayBetweenAddSpeed)
            {
                timer = 0;
                GameManager.Instance.playerDatas.IncreaseSpeed(playerController.addSpeed);
            }
            
            // TODO : bounce
        }

        public void PhysicsUpdate(PlayerController playerController, float fixedDeltaTime)
        {

        }


        public void OnExitState(PlayerController playerController)
        {
            GameManager.Instance.playerDatas.ResetSpeed();
        }
    }
}
