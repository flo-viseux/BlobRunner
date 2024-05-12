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
        private NormalState _normalState;
        private BounceState _bounceState;
        private DiveState _diveState;
        private JumpState _jumpState;
        private ShrinkState _shrinkState;

        private float startTimeHold;
        
        public PlayerStateMachine(PlayerController controller, InputManager inputManager)
        {
            this.inputManager = inputManager;
            playerController = controller;
            
            _normalState = new NormalState();
            _bounceState = new BounceState();
            _diveState = new DiveState();
            _jumpState = new JumpState();
            _shrinkState = new ShrinkState();
            
            OnChangeState(_normalState);
        }
        
        public void OnChangeState(IPlayerState newState)
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
            inputManager.OnStartTouch += Shrink;
            inputManager.OnEndTouch += Bounce;
            inputManager.OnSwipeSuccessful += Dive;
            inputManager.OnTap += Jump;
        }

        public void UnsubscribeToInput()
        {
            inputManager.OnStartTouch -= Shrink;
            inputManager.OnEndTouch -= Bounce;
            inputManager.OnSwipeSuccessful -= Dive;
            inputManager.OnTap -= Jump;
        }

        private void Shrink(float startTime)
        {
            if (currentState is NormalState)
            {
                startTimeHold = startTime;
                OnChangeState(_shrinkState);
            }
        }

        private void Bounce(float endTime)
        {
            if (currentState is ShrinkState)
            {
                _bounceState.SetDurationTime(endTime - startTimeHold);
                OnChangeState(_bounceState);
            }
        }

        private void Dive()
        {
            if (currentState is NormalState)
            {
                OnChangeState(_diveState);
            }
        }

        private void Jump()
        {
            if (currentState is NormalState)
            {
                OnChangeState(_jumpState);
            }
        }
    }
}
