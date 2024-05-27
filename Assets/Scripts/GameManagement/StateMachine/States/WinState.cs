using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinState : IGameBaseState
{
    public GameStatus Status => GameStatus.WIN;

    public void OnEnterState()
    {
        Debug.Log("Enter Win State");
        UIManager.Instance.ShowUIPanel(Status);
    }

    public void OnExitState()
    {
        UIManager.Instance.HideUIPanel(Status);
    }
}
