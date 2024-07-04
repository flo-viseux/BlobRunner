using System;
using UnityEngine;

namespace Runner.Player
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] private ContactFilter2D filter;
        
        private Collider2D[] results = new Collider2D[5];
        
        private bool isOnGround;
        private bool wasOnGround;
        private bool currentGroundCheck;
        
        public event Action<bool, Collider2D> OnGround;

        private void Start()
        {
            isOnGround = false;
            currentGroundCheck = false;
        }

        private void Update()
        {
            wasOnGround = isOnGround;
            currentGroundCheck =
                Physics2D.OverlapBox(transform.position, transform.localScale, 0f, filter, results) > 0;

            if (currentGroundCheck != wasOnGround)
            {
                isOnGround = currentGroundCheck;
                OnGround?.Invoke(isOnGround, isOnGround ? results[0] : null);
                //Debug.Log("is on ground : "+isOnGround);
            }
        }
    }
}