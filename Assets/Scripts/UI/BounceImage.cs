using System;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class BounceImage : MonoBehaviour
    {
        public float bounceScale = 1.2f;
        public float bounceDuration = 0.5f;

        private Tween bounceTween;

        private void OnEnable()
        {
            // Start the bounce animation
            bounceTween = transform.DOScaleY(bounceScale, bounceDuration)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDisable()
        {
            // Stop the bounce animation
            if (bounceTween != null)
            {
                bounceTween.Kill();
            }
        }
    }
}