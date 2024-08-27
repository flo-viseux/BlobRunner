using System.Collections;
using GameManagement;
using UnityEngine;
using Runner.Player;
using UnityEngine.Audio;
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
    public bool isPause = false;

    [SerializeField] private SimpleEventSO hitObstacle;
    [SerializeField] private SimpleEventSO winEvent;

    [SerializeField] private SaveLevelScore saveLevelScore;
    [SerializeField] private AudioMixer mixer;

    private AudioManager _audioManager;
    public CoroutineStorage coroutineStorage;
    
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
        _audioManager = new AudioManager(mixer);
        // no music in menu
        _audioManager.SetParamVolume(AudioManager.GroupType.Ambient, -80f);
        coroutineStorage = new CoroutineStorage(10);
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
            case GameStatus.MENU : 
                stateMachine.OnChangeState(_menuState);
                return;
            case GameStatus.LEVELMENU: 
                stateMachine.OnChangeState(_levelMenuState);
                return;
            case GameStatus.PAUSE : 
                stateMachine.OnChangeState(_pauseState);
                return;
            case GameStatus.GAME :
                if (!wasPaused)
                    _gameState = new GameState(playerDatas, gameSceneName);
                stateMachine.OnChangeState(_gameState);
                StartCoroutine(WaitBeforeStartSFXRoutine(0.3f));
                return;
            case GameStatus.WIN :
                saveLevelScore.OnVictory(levelIndex, playerDatas.CollectiblesCount, SectionGenerator.Instance.TotalCollectiblesCount);
                stateMachine.OnChangeState(_winState);
                return;
            case GameStatus.LOOSE : 
                saveLevelScore.OnGameOver(levelIndex, SectionGenerator.Instance.TotalCollectiblesCount);
                stateMachine.OnChangeState(_looseState);
                if (SceneManager.GetSceneByName(gameSceneName).isLoaded)
                    SceneManager.UnloadScene(gameSceneName);
                //SwitchState(GameStatus.LOAD);
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
        // audio : mute sfx, 
        mixer.SetFloat("SFX_Volume", -80f);
        
        // if ambiant music is mute, comes from menu, unmute music for game
        if (!_audioManager.IsParamPlaying(AudioManager.GroupType.Ambient))
        {
            _audioManager.ChangeVolume(AudioManager.GroupType.Ambient, -5f, 0.8f);
        }
        
        // unload - load scene
        yield return new WaitForSeconds(0.5f);

        // if (coroutineStorage.Length != 0)
        // {
        //     for (int i = 0; i < coroutineStorage.Length; i++)
        //     {
        //         StopCoroutine(coroutineStorage.GameRoutines[i]);
        //     }
        //     coroutineStorage.ClearRoutines();
        // }
        
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
        _audioManager.ChangeVolume(AudioManager.GroupType.Ambient, -80f, 0.2f);
        SwitchState(GameStatus.MENU);
    }

    public void GoToLevelMenu()
    {
        StartCoroutine(WaitBeforeLevelRoutine(0.3f));
    }

    public void GoToPause()
    {
        wasPaused = true;
        SwitchState(GameStatus.PAUSE);
    }

    public void Resume()
    {
        StartCoroutine(WaitBeforeResumeRoutine(0.2f));
        //SwitchState(GameStatus.GAME);
    }
    
    public void Restart()
    {
        wasPaused = false;
        StartCoroutine(WaitBeforeGameRoutine(0.2f));
        //SwitchState(GameStatus.LOAD);
    }

    public void GoToGame()
    {
        StartCoroutine(WaitBeforeGameRoutine(0.5f));
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
    
    #region Wait Routine

    private IEnumerator WaitBeforeLooseRoutine(float time)
    {
        SectionGenerator.Instance.Scrolling = false;
        yield return new WaitForSeconds(time);
        if (coroutineStorage.Length != 0)
        {
            for (int i = 0; i < coroutineStorage.Length; i++)
            {
                StopCoroutine(coroutineStorage.GameRoutines[i]);
            }
            coroutineStorage.ClearRoutines();
        }
        SwitchState(GameStatus.LOOSE);
    }

    private IEnumerator WaitBeforeWinRoutine(float time)
    {
        SectionGenerator.Instance.Scrolling = false;
        yield return new WaitForSeconds(time);
        SwitchState(GameStatus.WIN);
    }
    
    private IEnumerator WaitBeforeLevelRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        SwitchState(GameStatus.LEVELMENU);
    }
    
    private IEnumerator WaitBeforeGameRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        SwitchState(GameStatus.LOAD);
    }
    
    private IEnumerator WaitBeforeResumeRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        SwitchState(GameStatus.GAME);
    }

    private IEnumerator WaitBeforeStartSFXRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        mixer.SetFloat("SFX_Volume", 0f);
    }
    #endregion
}
