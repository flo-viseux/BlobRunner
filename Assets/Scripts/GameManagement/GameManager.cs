using System.Collections;
using UnityEngine;
using Runner.Player;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    Debug.LogError("Game manager not found : Persistant scene not loaded");
                }
            }

            return _instance;
        }
    }
    
    private GameStateMachine stateMachine;
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
    public float loadingTime = 0.5f;

    [SerializeField] private SimpleEventSO hitObstacle;
    [SerializeField] private SimpleEventSO winEvent;

    [SerializeField] private SaveLevelScore saveLevelScore;

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
        if (playerDatas.CurrentHealth <= 0)
        {
            GoToLoose();
        }
    }

    public void SwitchState(GameStatus newGameStatus)
    {
        switch (newGameStatus)
        {
            case GameStatus.MENU : stateMachine.OnChangeState(_menuState);
                return;
            case GameStatus.LEVELMENU: stateMachine.OnChangeState(_levelMenuState);
                return;
            case GameStatus.PAUSE : stateMachine.OnChangeState(_pauseState);
                return;
            case GameStatus.GAME :
                _gameState = new GameState(playerDatas, gameSceneName);
                stateMachine.OnChangeState(_gameState);
                return;
            case GameStatus.WIN :
                saveLevelScore.OnVictory(levelIndex, playerDatas.CollectiblesCount, SectionGenerator.Instance.TotalCollectiblesCount);
                stateMachine.OnChangeState(_winState);
                return;
            case GameStatus.LOOSE : 
                saveLevelScore.OnGameOver(levelIndex, SectionGenerator.Instance.TotalCollectiblesCount);
                stateMachine.OnChangeState(_looseState);
                return;
            case GameStatus.LOAD :
                _loadState = new LoadState(gameSceneName, loadingTime);
                stateMachine.OnChangeState(_loadState);
                StartCoroutine(LoadUpdate(_loadState));
                return;
            default: return;
        }
    }

    private IEnumerator LoadUpdate(LoadState state)
    {
        yield return StartCoroutine(state.UnloadScene());
        yield return StartCoroutine(state.LoadScene());
        SwitchState(GameStatus.GAME);
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
        wasPaused = false;
        SwitchState(GameStatus.LOAD);
    }

    public void GoToGame()
    {
        SwitchState(GameStatus.LOAD);
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
        SectionGenerator.Instance.Scrolling = false;
        yield return new WaitForSeconds(time);
        SwitchState(GameStatus.LOOSE);
    }

    private IEnumerator WaitBeforeWinRoutine(float time)
    {
        SectionGenerator.Instance.Scrolling = false;
        yield return new WaitForSeconds(time);
        SwitchState(GameStatus.WIN);
    }
}
