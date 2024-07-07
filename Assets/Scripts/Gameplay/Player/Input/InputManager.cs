
using UnityEngine;

namespace Runner.Player
{
    public class InputManager : MonoBehaviour
    { 
        public delegate void StartTouchEvent(float time);
        public event StartTouchEvent OnStartTouch;
        
        public delegate void EndTouchEvent(float time);
        public event EndTouchEvent OnEndTouch;

        public delegate void HoldEvent(float time);
        public event HoldEvent OnHold;

        public delegate void SwipeSuccesfulEvent();
        public event SwipeSuccesfulEvent OnSwipeSuccessful;
        
        public delegate void TapEvent();
        public event TapEvent OnTap;


        void OnEnable()
        {
            Lean.Touch.LeanTouch.OnFingerSwipe += HandleSwipe;
            Lean.Touch.LeanTouch.OnFingerUpdate += HandleFingerHold;
            Lean.Touch.LeanTouch.OnFingerTap += HandleTap;
            Lean.Touch.LeanTouch.OnFingerOld += HandleFingerOld;
            Lean.Touch.LeanTouch.OnFingerUp += HandleFingerUp;
        }

        void OnDisable()
        {
            Lean.Touch.LeanTouch.OnFingerSwipe -= HandleSwipe;
            Lean.Touch.LeanTouch.OnFingerUpdate -= HandleFingerHold;
            Lean.Touch.LeanTouch.OnFingerTap -= HandleTap;
            Lean.Touch.LeanTouch.OnFingerOld -= HandleFingerOld;
            Lean.Touch.LeanTouch.OnFingerUp -= HandleFingerUp;
        }

        void HandleFingerOld(Lean.Touch.LeanFinger finger)
        {
            if (!GameManager.Instance.wasPaused)
            {
                if (finger.Index != 0) return;

                //Debug.Log($"Hold {finger.Age} {finger.Index}");

                if (finger.Age > 1f) return;
                OnStartTouch(finger.Age);
            }
        }

        void HandleFingerHold(Lean.Touch.LeanFinger finger)
        {
            if (!GameManager.Instance.wasPaused)
            {
                if (finger.Index != 0) return;
        
                OnHold(finger.Age);
            }
        }

        void HandleFingerUp(Lean.Touch.LeanFinger finger)
        {
            if (!GameManager.Instance.wasPaused)
            {
                if (finger.Index != 0) return;

                //Debug.Log($"Hold stop {finger.Age} {finger.Index}");
                OnEndTouch(finger.Age);
            }
        }

        void HandleSwipe(Lean.Touch.LeanFinger finger)
        {
            if (!GameManager.Instance.wasPaused)
            {
                if (finger.Index != 0) return;

                Vector2 swipeDelta = finger.SwipeScreenDelta;

                if (swipeDelta.y < -Mathf.Abs(swipeDelta.x))
                {
                    //Debug.Log("Swiped Down");
                    // Debug.Log($"Swiped Down {finger.Age} {finger.Index}");
                    OnSwipeSuccessful();
                }
            }
        }

        void HandleTap(Lean.Touch.LeanFinger finger)
        {
            if (!GameManager.Instance.wasPaused)
            {
                if (finger.Index != 0) return;

                //Debug.Log("Tap");
                // Debug.Log($" Tap {finger.Age} {finger.Index}");
                OnTap();
            }


        }
    }
}
