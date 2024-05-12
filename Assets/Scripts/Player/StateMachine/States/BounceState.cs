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
        
        public void OnEnterState(PlayerController playerController)
        {
            throw new System.NotImplementedException();
        }

        public void LogicUpdate(PlayerController playerController)
        {
            throw new System.NotImplementedException();
        }

        public void PhysicsUpdate(PlayerController playerController)
        {
            throw new System.NotImplementedException();
        }


        public void OnExitState(PlayerController playerController)
        {
            throw new System.NotImplementedException();
        }
    }
}
