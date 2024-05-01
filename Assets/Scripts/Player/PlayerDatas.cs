
using UnityEngine;

[System.Serializable]
public class PlayerDatas
{
    public int MaxHealth = 1;
    [HideInInspector] public int CurrentHealth;

    public float StartSpeed = 5f;
    [HideInInspector] public float CurrentSpeed;

    [HideInInspector] public int CurrentScore;

    public void InitPlayerDatas()
    {
        CurrentHealth = MaxHealth;
        CurrentSpeed = StartSpeed;
        CurrentScore = 0;
    }
}