using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadState : IGameBaseState
{
    public GameStatus Status => GameStatus.LOAD;

    public void OnEnterState()
    {
        //Debug.Log("Enter Load State");
        UIManager.Instance.ShowUIPanel(Status);
    }

    public void OnExitState()
    {
        UIManager.Instance.HideUIPanel(Status);
    }
}
