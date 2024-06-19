using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Player
{
    public class ShrinkState : IPlayerState
    {
        private float coefSize = 2f; // or switch sprite
        private float holdDuration;
        public void OnEnterState(PlayerController playerController)
        {
            //Debug.Log("Shrink state");
            // TODO :sound shrink
            holdDuration = 0f;
            
            // change scale for now
            Vector3 scale = playerController.spriteTransform.localScale / coefSize;
            playerController.spriteTransform.localScale = scale;
        }

        public void LogicUpdate(PlayerController playerController, float deltaTime)
        {
            if (holdDuration > playerController.maxHoldTime) return;
            holdDuration += deltaTime;
            playerController.GetJumpChargeUI().UpdateCharge(holdDuration);
        }

        public void PhysicsUpdate(PlayerController playerController, float fixedDeltaTime)
        {

        }

        public void OnExitState(PlayerController playerController)
        {
            holdDuration = 0f;
            playerController.GetJumpChargeUI().ResetSlider();
            // switch scale back to normal
            Vector3 scale = playerController.spriteTransform.localScale * coefSize;
            playerController.spriteTransform.localScale = scale;
            
            // TODO : sound back to normal
        }
    }

}