using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance != null || Instance != this)
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
            case GameStatus.MENU : return gameObject;
            case GameStatus.PAUSE : return gameObject;
            case GameStatus.LOAD : return gameObject;
            case GameStatus.GAME : return gameObject;
            case GameStatus.WIN : return gameObject;
            case GameStatus.LOOSE : return gameObject;
            default: return gameObject;
        }
    }
}
