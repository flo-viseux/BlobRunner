using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Runner.Player
{
    public class DiveState : IPlayerState
    {
        public void OnEnterState(PlayerController playerController)
        {
            //Debug.Log("Dive State");
            // TODO : sound
            Vector2 diveImpulse = new Vector2(0f,-1 * playerController.diveForce);
            playerController.rb2D.AddForce(diveImpulse, ForceMode2D.Impulse);
        }

        public void LogicUpdate(PlayerController playerController, float deltaTime)
        {
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
            // TODO : sound land
            
            if (playerController.isBouncingFromChemical)
                playerController.isBouncingFromChemical = false;

        }
    }
}
