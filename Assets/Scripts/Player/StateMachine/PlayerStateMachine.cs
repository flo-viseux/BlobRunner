using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Runner.Player
{
    public class PlayerStateMachine
    {
        public IPlayerState currentState;
        private InputManager inputManager;
        private PlayerController playerController;
        private BounceState _bounceState;
        private DiveState _diveState;
        private JumpState _jumpState;
        private ShrinkState _shrinkState;

        public PlayerStateMachine(PlayerController controller, InputManager inputManager)
        {
            this.inputManager = inputManager;
            playerController = controller;
            _bounceState = new BounceState();
            _diveState = new DiveState();
            _jumpState = new JumpState();
            _shrinkState = new ShrinkState();
        }
        
        private void OnChangeState(IPlayerState newState)
        {
            if (currentState != null)
            {
                currentState.OnExitState(playerController);
            }

            currentState = newState;
            currentState.OnEnterState(playerController);
        }

        private void SubscribeToInput()
        {
            // inputManager.OnStartTouch += ;
            // inputManager.OnEndTouch += ;
            // inputManager.OnSwipeSuccessful += ;
        }

        public void UnsubscribeToInput()
        {
            
        }
        

    }
}
