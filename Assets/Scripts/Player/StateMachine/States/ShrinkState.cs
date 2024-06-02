using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Player
{
    public class ShrinkState : IPlayerState
    {
        private float coefSize = 2f; // or switch sprite
        
        public void OnEnterState(PlayerController playerController)
        {
            Debug.Log("Shrink state");
            // TODO :sound shrink
            
            playerController.rb2D.gravityScale = playerController.gravityFall;
            
            Vector3 scale = playerController.spriteTransform.localScale / coefSize;
            playerController.spriteTransform.localScale = scale;
        }

        public void LogicUpdate(PlayerController playerController, float deltaTime)
        {

        }

        public void PhysicsUpdate(PlayerController playerController, float fixedDeltaTime)
        {

        }

        public void OnExitState(PlayerController playerController)
        {
            playerController.rb2D.gravityScale = playerController.startGravity;
            
            Vector3 scale = playerController.spriteTransform.localScale * coefSize;
            playerController.spriteTransform.localScale = scale;
            
            // TODO : sound back to normal
        }
    }

}