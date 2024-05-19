using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Player
{
    public class DiveState : IPlayerState
    {
        public void OnEnterState(PlayerController playerController)
        {
            Debug.Log("Dive State");
            // sound
        }

        public void LogicUpdate(PlayerController playerController, float deltaTime)
        {
            playerController.transform.Translate(Vector3.down * (playerController.diveForce * deltaTime));
            
            if (playerController.IsOnGround())
            {
                playerController.stateMachine.NormalState();
                
            }
        }

        public void PhysicsUpdate(PlayerController playerController, float fixedDeltaTime)
        {

        }

        public void OnExitState(PlayerController playerController)
        {
            // sound land
            
            if (playerController.isBouncingFromChemical)
                playerController.isBouncingFromChemical = false;

        }
    }
}
