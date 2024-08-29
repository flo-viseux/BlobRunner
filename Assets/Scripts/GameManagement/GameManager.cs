using System;
using System.Collections;
using UnityEngine;
using Runner.Player;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public GameStateMachine stateMachine;
    private MenuState _menuState;
    private LevelMenuState _levelMenuState;
    private LoadState _loadState;
    private GameState _gameState;
    private PauseState _pauseState;
    private WinState _winState;
    private LooseState _looseState;

    public PlayerDatas playerDatas;
    public string gameSceneName;
    public int levelIndex;

    public bool wasPaused;
    public float loadingTime = 0.1f;

    [SerializeField] private SimpleEventSO hitObstacle;
    [SerializeField] private SimpleEventSO winEvent;

    [SerializeField] private SaveLevelScore saveLevelScore;

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

    private void OnEnable()
    {
        hitObstacle.OnEventRaised += LoseLife;
        winEvent.OnEventRaised += GoToWin;
    }

    private void OnDisable()
    {
        hitObstacle.OnEventRaised -= LoseLife;
        winEvent.OnEventRaised -= GoToWin;
    }

    private void Start()
    {
        stateMachine = new GameStateMachine();
        _menuState = new MenuState();
        _levelMenuState = new LevelMenuState();
        _gameState = new GameState(playerDatas, gameSceneName);
        _pauseState = new PauseState();
        _winState = new WinState();
        _looseState = new LooseState();
        stateMachine.OnChangeState(_menuState);

        wasPaused = false;
    }

    
    private void LoseLife()
    {
        playerDatas.DecreaseHealth();
    }

    public void SwitchState(GameStatus newGameStatus)
    {
        switch (newGameStatus)
        {
            case GameStatus.MENU : stateMachine.OnChangeState(_menuState);
                return;
            case GameStatus.LEVELMENU: stateMachine.OnChangeState(_levelMenuState);
                return;
            case GameStatus.RESTART:
                saveLevelScore.OnGameOver(levelIndex, SectionGenerator.Instance.TotalCollectiblesCount);
                StartCoroutine(LoadScreen());
                _gameState = new GameState(playerDatas, gameSceneName);
                stateMachine.OnChangeState(_gameState);
                return;
            case GameStatus.PAUSE : stateMachine.OnChangeState(_pauseState);
                return;
            case GameStatus.GAME :
                if (!wasPaused)
                {
                    StartCoroutine(LoadScreen());
                    _gameState = new GameState(playerDatas, gameSceneName);
                }
                stateMachine.OnChangeState(_gameState);
                return;
            case GameStatus.WIN :
                saveLevelScore.OnVictory(levelIndex, playerDatas.CollectiblesCount, SectionGenerator.Instance.TotalCollectiblesCount);
                stateMachine.OnChangeState(_winState);
                return;
            case GameStatus.LOOSE : stateMachine.OnChangeState(_looseState);
                saveLevelScore.OnGameOver(levelIndex, SectionGenerator.Instance.TotalCollectiblesCount);
                StartCoroutine(LoadScreen());
                _gameState = new GameState(playerDatas, gameSceneName);
                stateMachine.OnChangeState(_gameState);
                return;
            default: return;
        }
    }
    
    private IEnumerator LoadScreen()
    {
        UIManager.Instance.ShowUIPanel(GameStatus.LOAD);
        AsyncOperation async = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);
        
        float startLoadingTime = Time.time;
        
        while (!async.isDone) yield return null;
        
        // Make sure the load screen doesn't flash if the loading is fast, wait 0.5s
        float loadedTime = Time.time - startLoadingTime;
        if (loadedTime < loadingTime)
        {
            yield return new WaitForSeconds(loadingTime - loadedTime);
            // TODO : prevent game to start while loading panel isn't hidden
        }

        UIManager.Instance.HideUIPanel(GameStatus.LOAD);
    }


    public void SetWasPaused(bool value)
    {
        StartCoroutine(SetWasPausedCoroutine(value));
    }

    public IEnumerator SetWasPausedCoroutine(bool value)
    {
        yield return null;

        wasPaused = value;
    }

    #region Buttons

    public void GoToMenu()
    {
        if (SceneManager.GetSceneByName(gameSceneName).isLoaded)
            SceneManager.UnloadScene(gameSceneName);
        wasPaused = false;
        SwitchState(GameStatus.MENU);
    }

    public void GoToLevelMenu()
    {
        SwitchState(GameStatus.LEVELMENU);
    }

    public void GoToPause()
    {
        wasPaused = true;
        SwitchState(GameStatus.PAUSE);
    }
    public void Restart()
    {
        if (SceneManager.GetSceneByName(gameSceneName).isLoaded)
            SceneManager.UnloadScene(gameSceneName);
        wasPaused = false;
        SwitchState(GameStatus.RESTART);
    }

    public void GoToGame()
    {
        SwitchState(GameStatus.GAME);
    }

    public void GoToWin()
    {
        StartCoroutine(WaitBeforeWinRoutine(0.5f));
    }

    public void GoToLoose()
    {
        StartCoroutine(WaitBeforeLooseRoutine(0.5f));
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    private IEnumerator WaitBeforeLooseRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        SwitchState(GameStatus.LOOSE);
    }

    private IEnumerator WaitBeforeWinRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        SwitchState(GameStatus.WIN);
    }
}
