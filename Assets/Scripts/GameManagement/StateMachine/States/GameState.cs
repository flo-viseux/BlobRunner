using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : IGameBaseState
{
    public GameStatus Status => GameStatus.GAME;
    private string sceneName = ""; // TODO : give the name of the game scene
    public void OnEnterState()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        UIManager.Instance.ShowUIPanel(Status);
    }

    public void OnExitState()
    {
        SceneManager.UnloadSceneAsync(sceneName);
        UIManager.Instance.HideUIPanel(Status);
    }
}
