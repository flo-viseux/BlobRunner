using System;
using System.Linq;
using UnityEngine;

namespace Runner.Player
{
    public class GroundCheck : MonoBehaviour
    {
        private bool canCheckGround = false;

        // ground
        [SerializeField] private ContactFilter2D filter;

        private Collider2D[] results = new Collider2D[5];

        private bool isOnGround;
        private bool wasOnGround;
        private bool currentGroundCheck;
        private int groundCount;

        public event Action<bool, Collider2D> OnGround;

        // head
        [SerializeField] private Transform headSpriteTransform;
        public event Action<bool> OnHitHead;
        private Collider2D[] resultsHead = new Collider2D[3];

        private bool hasHitHead;
        private bool currentHitHead;

        private void Start()
        {
            canCheckGround = true;

            // ground
            isOnGround = false;
            wasOnGround = false;
            currentGroundCheck = false;
            groundCount = 0;

            // head
            hasHitHead = false;
        }

        private void Update()
        {
            // ground
            groundCount = Physics2D.OverlapBox(transform.position, transform.localScale, 0f, filter, results);
            currentGroundCheck = groundCount > 0;

            if (currentGroundCheck != wasOnGround)
            {
                if (groundCount > 0)
                {
                    OnGround?.Invoke(true, results[0]);
                    wasOnGround = true;
                }
                else
                {
                    OnGround?.Invoke(false, null);
                    wasOnGround = false;
                }
            }
            ClearResults(results);

            // head
            int headCount = Physics2D.OverlapBox(headSpriteTransform.position, headSpriteTransform.localScale, 0f, filter, resultsHead);
            currentHitHead = headCount > 0;

            if (currentHitHead != hasHitHead)
            {
                if (groundCount > 0)
                {
                    OnHitHead?.Invoke(false);
                }
                else
                {
                    OnHitHead?.Invoke(true);
                }

                hasHitHead = currentHitHead;
            }
            ClearResults(resultsHead);
        }

        private void ClearResults(Collider2D[] colliders)
        {
            if (colliders == null) return;
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i] = null;
            }
        }

        public void ChangeWasOnGround(bool wasGround)
        {
            wasOnGround = wasGround;
        }
    }
}