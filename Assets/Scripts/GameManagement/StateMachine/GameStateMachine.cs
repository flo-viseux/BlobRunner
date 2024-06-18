using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine
{
    public IGameBaseState currentState;

    public void OnChangeState(IGameBaseState nextState)
    {
        if (currentState != null)
            currentState.OnExitState();
        currentState = nextState;
        currentState.OnEnterState();
    }
}
