using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        stateMachine = new GameStateMachine();
        _menuState = new MenuState();
        _loadState = new LoadState();
        _gameState = new GameState();
        _pauseState = new PauseState();
        _winState = new WinState();
        _looseState = new LooseState();
        stateMachine.OnChangeState(_menuState);
        
    }

    public void SwitchState(GameStatus newGameStatus)
    {
        switch (newGameStatus)
        {
            case GameStatus.MENU : stateMachine.OnChangeState(_menuState);
                return;
            case GameStatus.PAUSE : stateMachine.OnChangeState(_pauseState);
                return;
            case GameStatus.LOAD : stateMachine.OnChangeState(_loadState);
                return;
            case GameStatus.GAME : 
                stateMachine.OnChangeState(_gameState);
                playerDatas.InitPlayerDatas();
                return;
            case GameStatus.WIN : stateMachine.OnChangeState(_winState);
                return;
            case GameStatus.LOOSE : stateMachine.OnChangeState(_looseState);
                return;
            default: return;
        }
    }
}
