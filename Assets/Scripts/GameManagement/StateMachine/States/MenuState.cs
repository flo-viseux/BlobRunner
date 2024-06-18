using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : IGameBaseState
{
    public GameStatus Status => GameStatus.MENU;

    public void OnEnterState()
    {
        //Debug.Log("Enter Menu State");
        UIManager.Instance.ShowUIPanel(Status);
    }

    public void OnExitState()
    {
        UIManager.Instance.HideUIPanel(Status);
    }
}
