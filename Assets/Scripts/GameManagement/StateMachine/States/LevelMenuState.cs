using UnityEngine;

public class LevelMenuState : IGameBaseState
{
    public GameStatus Status => GameStatus.LEVELMENU;

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
