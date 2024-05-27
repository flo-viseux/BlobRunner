using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : IGameBaseState
{
    public GameStatus Status => GameStatus.PAUSE;

    public void OnEnterState()
    {
        Debug.Log("Enter Pause State");
        Time.timeScale = 0f;
        UIManager.Instance.ShowUIPanel(Status);
    }

    public void OnExitState()
    {
        Time.timeScale = 1f;
        UIManager.Instance.HideUIPanel(Status);
        GameManager.Instance.wasPaused = true;
    }
}
