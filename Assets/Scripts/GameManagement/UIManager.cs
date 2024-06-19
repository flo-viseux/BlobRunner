using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("UI Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject loosePanel;
    [SerializeField] private GameObject pausePanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void HideUIPanel(GameStatus status)
    {
        GetPanelFromGameStatus(status).SetActive(false);
    }

    public void ShowUIPanel(GameStatus status)
    {
        GetPanelFromGameStatus(status).SetActive(true);
    }
    
    private GameObject GetPanelFromGameStatus(GameStatus status)
    {
        // TODO : change game object to UI panel
        switch (status)
        {
            case GameStatus.MENU : return menuPanel;
            case GameStatus.PAUSE : return pausePanel;
            case GameStatus.LOAD : return loadingPanel;
            case GameStatus.GAME : return gamePanel;
            case GameStatus.WIN : return winPanel;
            case GameStatus.LOOSE : return loosePanel;
            default: return gameObject;
        }
    }
}
