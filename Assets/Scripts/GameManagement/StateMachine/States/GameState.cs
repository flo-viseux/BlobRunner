using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.Player;
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
        //Debug.Log("Enter Game State");
        if (!GameManager.Instance.wasPaused)
        {
            playerDatas.InitPlayerDatas();
        }
        UIManager.Instance.ShowUIPanel(Status);
        if (GameManager.Instance.wasPaused) GameManager.Instance.SetWasPaused(false);
    }

    public void OnExitState()
    {
        if (!GameManager.Instance.wasPaused)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
        UIManager.Instance.HideUIPanel(Status);
    }


}
