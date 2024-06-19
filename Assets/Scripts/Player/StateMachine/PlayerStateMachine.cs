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
        private InAirState _inAirState;

        private float startTimeHold;
        private float endTimeHold;

        public bool tapAllowed = false;
        
        public PlayerStateMachine(PlayerController controller, InputManager inputManager)
        {
            this.inputManager = inputManager;
            playerController = controller;
            
            _normalState = new NormalState();
            _bounceState = new BounceState();
            _diveState = new DiveState();
            _jumpState = new JumpState();
            _shrinkState = new ShrinkState();
            _inAirState = new InAirState();
            
            OnChangeState(_normalState);
            tapAllowed = false;
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

        public void SubscribeToInput()
        {
            inputManager.OnStartTouch += Shrink;
            inputManager.OnEndTouch += GetHoldTime;
            inputManager.OnSwipeSuccessful += Dive;
            inputManager.OnTap += JumpBack;
        }

        public void UnsubscribeToInput()
        {
            inputManager.OnStartTouch -= Shrink;
            inputManager.OnEndTouch -= GetHoldTime;
            inputManager.OnSwipeSuccessful -= Dive;
            inputManager.OnTap -= JumpBack;
        }

        private void GetHoldTime(float endTime)
        {
            endTimeHold = endTime;
            Jump();
        }

        private void Shrink(float startTime)
        {
            if (currentState is NormalState && playerController.IsOnGround())
            {
                startTimeHold = startTime;
                OnChangeState(_shrinkState);
                CameraSwitcher.Instance.SwitchCamera();
            }
        }

        public void Bounce()
        {
            if (currentState is JumpState)
            {
                OnChangeState(_bounceState);
            }
        }

        private void Dive()
        {
            OnChangeState(_diveState);
        }

        private void Jump()
        {
            if (currentState is NormalState)
            {
                _jumpState.isTap = true;
                OnChangeState(_jumpState);
            }
            else if (currentState is ShrinkState)
            {
                _jumpState.isTap = false;
                _jumpState.SetDurationTime(endTimeHold - startTimeHold);
                OnChangeState(_jumpState);

                CameraSwitcher.Instance.SwitchCamera();
                CameraShaker.Instance.Shake();
            }
        }

        private void JumpBack()
        {
            Debug.Log($"jump back tap allowed : {tapAllowed}");
            if (currentState is InAirState && tapAllowed)
            {
                ((InAirState)currentState).hasTapped = true;
            }
        }

        public void NormalState()
        {
            OnChangeState(_normalState);
        }

        public void InAir()
        {
            if (currentState is JumpState)
            {
                OnChangeState(_inAirState);
            }
        }
    }
}
