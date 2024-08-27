using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private FadePanel fadePanel;
    [Header("UI Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject levelMenuPanel;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject loosePanel;
    [SerializeField] private GameObject pausePanel;

    public event Action<bool> pauseEvent;
    
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
                if (_instance == null)
                {
                    Debug.LogError("UI manager not found : Persistant scene not loaded");
                }
            }

            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void HideUIPanel(GameStatus status, bool activeFade = false, float fadeDuration = 0f, float stayBlackDuration = 0f)
    {
        if (status == GameStatus.PAUSE)
        {
            pauseEvent?.Invoke(false);
        }
        
        if (activeFade) fadePanel.FadeIn(fadeDuration, stayBlackDuration);
        GetPanelFromGameStatus(status).SetActive(false);
    }

    public void ShowUIPanel(GameStatus status, bool activeFade = false, float fadeDuration = 0f, float stayBlackDuration = 0f)
    {
        if (status == GameStatus.PAUSE)
        {
            pauseEvent?.Invoke(true);
        }
        if (activeFade) fadePanel.FadeOut(fadeDuration, stayBlackDuration);
        GetPanelFromGameStatus(status).SetActive(true);
    }
    
    private GameObject GetPanelFromGameStatus(GameStatus status)
    {
        switch (status)
        {
            case GameStatus.MENU : return menuPanel;
            case GameStatus.LEVELMENU: return levelMenuPanel;
            case GameStatus.PAUSE : return pausePanel;
            case GameStatus.LOAD : return loadingPanel;
            case GameStatus.GAME : return gamePanel;
            case GameStatus.WIN : return winPanel;
            case GameStatus.LOOSE : return loosePanel;
            default: return gameObject;
        }
    }
}
