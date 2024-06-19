using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Player
{
    public class NormalState : IPlayerState
    {
        public void OnEnterState(PlayerController playerController)
        {
            Debug.Log("Normal State");
        }

        public void LogicUpdate(PlayerController playerController, float deltaTime)
        {
            // TODO : sound walk  
        }

        public void PhysicsUpdate(PlayerController playerController, float fixedDeltaTime)
        {

        }

        public void OnExitState(PlayerController playerController)
        {
            // TODO :stop sound
        }
    }
}
