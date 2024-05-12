using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Runner.Player;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public GameStateMachine stateMachine;
    private MenuState _menuState;
    private LoadState _loadState;
    private GameState _gameState;
    private PauseState _pauseState;
    private WinState _winState;
    private LooseState _looseState;

    public PlayerDatas playerDatas;
    public string gameSceneName;

    public bool wasPaused;
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

    private void Start()
    {
        stateMachine = new GameStateMachine();
        _menuState = new MenuState();
        _gameState = new GameState(playerDatas, gameSceneName);
        _pauseState = new PauseState();
        _winState = new WinState();
        _looseState = new LooseState();
        stateMachine.OnChangeState(_menuState);

        wasPaused = false;
    }

    public void SwitchState(GameStatus newGameStatus)
    {
        switch (newGameStatus)
        {
            case GameStatus.MENU : stateMachine.OnChangeState(_menuState);
                return;
            case GameStatus.PAUSE : stateMachine.OnChangeState(_pauseState);
                return;
            case GameStatus.GAME :
                if (!wasPaused)
                {
                    StartCoroutine(LoadScreen());
                }
                stateMachine.OnChangeState(_gameState);
                return;
            case GameStatus.WIN : stateMachine.OnChangeState(_winState);
                return;
            case GameStatus.LOOSE : stateMachine.OnChangeState(_looseState);
                return;
            default: return;
        }
    }
    
    private IEnumerator LoadScreen()
    {
        UIManager.Instance.ShowUIPanel(GameStatus.LOAD);
        AsyncOperation async = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);
        while (!async.isDone) yield return null;
        UIManager.Instance.HideUIPanel(GameStatus.LOAD);
    }
    
    #region Buttons

    public void GoToMenu()
    {
        SwitchState(GameStatus.MENU);
    }

    public void GoToPause()
    {
        SwitchState(GameStatus.PAUSE);
    }
    public void GoToGame()
    {
        SwitchState(GameStatus.GAME);
    }

    public void GoToWin()
    {
        SwitchState(GameStatus.WIN);
    }

    public void GoToLoose()
    {
        SwitchState(GameStatus.LOOSE);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}
