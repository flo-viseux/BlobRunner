using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseState : IGameBaseState
{
    public GameStatus Status => GameStatus.LOOSE;

    public void OnEnterState()
    {
        //Debug.Log("Enter Loose State");
        UIManager.Instance.ShowUIPanel(Status);
    }

    public void OnExitState()
    {
        UIManager.Instance.HideUIPanel(Status);
    }
}
