using System;
using System.Linq;
using UnityEngine;

namespace Runner.Player
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] private ContactFilter2D filter;
        [SerializeField] private PlayerInvulnerability invulnerability;

        private Collider2D[] results = new Collider2D[5];

        private bool isOnGround;
        private bool wasOnGround;
        private bool currentGroundCheck;

        public event Action<bool, Collider2D> OnGround;

        private void Start()
        {
            isOnGround = false;
            wasOnGround = false;
            currentGroundCheck = false;
        }

        private void Update()
        {
            int groundCount = Physics2D.OverlapBox(transform.position, transform.localScale, 0f, filter, results);
            
            currentGroundCheck = groundCount > 0;
            if (results != null) ClearResults(groundCount);

            if (currentGroundCheck != wasOnGround)
            {
                
                Collider2D surface = GetSurfaceCollider(results);
                
                if (surface == null)
                {
                    OnGround?.Invoke(false, null);
                    wasOnGround = false;
                }

                else
                {
                    OnGround?.Invoke(true, surface);
                    wasOnGround = true;
                }
            }
        }



        private Collider2D GetSurfaceCollider(Collider2D[] colliders)
        {
            if (invulnerability.GetIsInvulnerable())
            {
                int nb = colliders.Count(x => x != null);

                for (int i = 0; i < nb; i++)
                {
                    if (!colliders[i].CompareTag("Obstacles"))
                    {
                        return colliders[i];
                    }
                }

                return null;
            }

            return colliders[0];
        }

        private void ClearResults(int groundCheckCount)
        {
            for (int i = groundCheckCount; i < results.Length; i++)
            {
                results[i] = null;
            }
        }
    }
}