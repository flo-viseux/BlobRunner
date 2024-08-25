using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : IGameBaseState
{
    public GameStatus Status => GameStatus.PAUSE;


    public void OnEnterState()
    {
        //Debug.Log("Enter Pause State");
        //Time.timeScale = 0f;
        SectionGenerator.Instance.Scrolling = false;
        UIManager.Instance.ShowUIPanel(Status);
        
    }

    public void OnExitState()
    {
        //Time.timeScale = 1f;
        SectionGenerator.Instance.Scrolling = true;
        UIManager.Instance.HideUIPanel(Status);
    }
}
