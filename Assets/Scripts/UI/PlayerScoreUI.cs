using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private PlayerScore playerScore = null;

    [SerializeField] private TMP_Text scoreLabel = null;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        playerScore.OnScoreChange += ScoreChange;
    }
    #endregion

    #region Private
    private void ScoreChange(int score)
    {
        scoreLabel.text = "Score : " + score.ToString();
    }
    #endregion
}
