
using System;
using System.Collections;
using UnityEngine;


namespace Runner.Player
{
    [System.Serializable]

    public class PlayerDatas
    {
        [SerializeField] private int MaxHealth = 1;
        [HideInInspector] public int CurrentHealth;

        [SerializeField] private float StartSpeed = 5f;
        [HideInInspector] public float CurrentSpeed;

        private int collectiblesCount;
        public int CollectiblesCount => collectiblesCount;

        public event Action<int> OnCollectiblesChange;

        public void InitPlayerDatas()
        {
            CurrentHealth = MaxHealth;
            CurrentSpeed = StartSpeed;
            collectiblesCount = 0;
        }

        public void IncreaseCollectiblesCount()
        {
            ++collectiblesCount;
            OnCollectiblesChange?.Invoke(collectiblesCount);
        }

        public void DecreaseScore()
        {
            --collectiblesCount;
            OnCollectiblesChange?.Invoke(collectiblesCount);
        }

        //public void ResetSpeed()
        //{
        //    CurrentSpeed = StartSpeed;
        //}

        //public void IncreaseSpeed(float value)
        //{
        //    CurrentSpeed += value;
        //}

        public void IncreaseHealth()
        {
            CurrentHealth++;
        }

        public void DecreaseHealth()
        {
            CurrentHealth--;

            if (CurrentHealth <= 0)
            {
                GameManager.Instance.GoToLoose();
            }
            //else
            //    DecreaseScore();
        }

    }
}