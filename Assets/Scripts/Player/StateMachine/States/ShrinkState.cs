using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Player
{
    public class ShrinkState : IPlayerState
    {
        public void OnEnterState(PlayerController playerController)
        {
            Debug.Log("Shrink state");
            // sound shrink
            playerController.rb2D.gravityScale = playerController.gravityFall;
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
        }
    }

}