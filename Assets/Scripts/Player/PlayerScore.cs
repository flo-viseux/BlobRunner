using System;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    #region Attributes
    private float score = 0f;
    #endregion
    
    #region API
    public static PlayerScore Instance;

    public event Action<int> OnScoreChange;

    public void IncreaseScore(float value)
    {
        score += value;

        OnScoreChange?.Invoke((int) score);
    }
    #endregion

    #region UnityMethods
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        score = 0f;
        IncreaseScore(0);
    }
    #endregion
}
