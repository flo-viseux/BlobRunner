using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private TMP_Text scoreLabel = null;
    #endregion

    #region UnityMethods
    private void OnEnable()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("Game Manager Instance is null");
            return;
        }

        GameManager.Instance.playerDatas.OnScroreChange += ScoreChange;
        scoreLabel.text = "Score : " + GameManager.Instance.playerDatas.Score.ToString();
    }

    #endregion

    #region Private
    private void ScoreChange(int score)
    {
        scoreLabel.text = "Score : " + score.ToString();
    }
    #endregion
}
