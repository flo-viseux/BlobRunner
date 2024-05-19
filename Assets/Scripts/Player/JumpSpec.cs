using UnityEngine;
using UnityEngine.UIElements;

namespace Runner.Player
{
    [CreateAssetMenu(fileName = "JumpSpec", menuName = "ScriptableObjects/Add Jump Spec", order = 0)]
    public class JumpSpec : ScriptableObject
    {
        public float initialJumpForce = 10f;
        [Tooltip("Curve used when hold")]
        public AnimationCurve gravityRise;
        [Tooltip("Curve used when falling")]
        public AnimationCurve gravityFall;
        [Tooltip("Used when released")]
        public float gravityOnRelease = 2f;
    }
}