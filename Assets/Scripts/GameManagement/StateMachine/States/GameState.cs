using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : IGameBaseState
{
    public GameStatus Status => GameStatus.GAME;
    private string sceneName;
    private PlayerDatas playerDatas;

    public GameState(PlayerDatas datas, string sceneNameToLoad)
    {
        playerDatas = datas;
        sceneName = sceneNameToLoad;
    }
    
    public void OnEnterState()
    {
        if (!GameManager.Instance.wasPaused)
        {
            playerDatas.InitPlayerDatas();
        }
        UIManager.Instance.ShowUIPanel(Status);
    }

    public void OnExitState()
    {
        if (!GameManager.Instance.wasPaused)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
        GameManager.Instance.wasPaused = false;
        UIManager.Instance.HideUIPanel(Status);
    }


}
