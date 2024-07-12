using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private TMP_Text collectiblesLabel = null;
    #endregion

    #region UnityMethods
    private void OnEnable()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("Game Manager Instance is null");
            return;
        }

        GameManager.Instance.playerDatas.OnCollectiblesChange += CollectiblesChange;
        collectiblesLabel.text = GameManager.Instance.playerDatas.CollectiblesCount.ToString();
    }

    #endregion

    #region Private
    private void CollectiblesChange(int collectibles)
    {
        collectiblesLabel.text = collectibles.ToString();
    }
    #endregion
}
