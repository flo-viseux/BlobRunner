using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine
{
    public IGameBaseState currentState;

    public void OnChangeState(IGameBaseState nextState)
    {
        currentState.OnExitState();
        currentState = nextState;
        currentState.OnEnterState();
    }
}
