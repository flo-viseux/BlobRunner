
using UnityEngine;

namespace Runner.Player
{
    [System.Serializable]

    public class PlayerDatas
    {
        [SerializeField] private int MaxHealth = 1;
        [HideInInspector] public int CurrentHealth;

        [SerializeField] private  float StartSpeed = 5f;
        [HideInInspector] public float CurrentSpeed;

        [HideInInspector] public int CurrentScore;

        public void InitPlayerDatas()
        {
            CurrentHealth = MaxHealth;
            CurrentSpeed = StartSpeed;
            CurrentScore = 0;
        }
    }
}