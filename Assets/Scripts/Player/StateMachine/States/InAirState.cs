using UnityEngine;


namespace Runner.Player
{
    public class InAirState : IPlayerState
    {
        private Vector2 normal;
        private float timer;
        private bool isTimerActive;
        public bool hasTapped;
        
        public void OnEnterState(PlayerController playerController)
        {
            Debug.Log("in Air State");
            playerController.OnHitGround += StartWindowToBounce;
            
            timer = 0;
            isTimerActive = false;
            hasTapped = false;
        }

        public void LogicUpdate(PlayerController playerController, float deltaTime)
        {
            // Allow Tap 
            if (isTimerActive)
            {
                PlayerController.stateMachine.tapAllowed = true;
                timer += deltaTime;
                
                if (timer > playerController.windowDurationBounce)
                {
                    PlayerController.stateMachine.tapAllowed = false;
                    isTimerActive = false;
                    if (!hasTapped)
                    {
                        PlayerController.stateMachine.NormalState();
                    } 
                }
            }
        }

        public void PhysicsUpdate(PlayerController playerController, float fixedDeltaTime)
        {
            if (hasTapped)
            {
                normal = normal.y > 0f ? Vector2.up : Vector2.down;
                Vector2 jumpImpulse = new Vector2(0f, playerController.jumpForce * normal.y);
                playerController.rb2D.AddForce(jumpImpulse, ForceMode2D.Impulse);
                
                hasTapped = false;
                isTimerActive = false;
                timer = 0f;
                Debug.Log($"tap : {normal} {jumpImpulse}");
                Debug.LogError("STOP");
            }
        }

        public void OnExitState(PlayerController playerController)
        {
            playerController.OnHitGround -= StartWindowToBounce;
            timer = 0f;
            Debug.Log("Exit in Air State");
        }
        
        private void StartWindowToBounce(Vector2 newDirection)
        {
            

            isTimerActive = true;
            normal = newDirection;
        }
    }
}