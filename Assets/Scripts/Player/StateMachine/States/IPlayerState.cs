using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Player
{
    public interface IPlayerState
    {
        public void OnEnterState(PlayerController playerController);
        public void LogicUpdate(PlayerController playerController);
        public void PhysicsUpdate(PlayerController playerController);
        public void OnExitState(PlayerController playerController);
    }

}