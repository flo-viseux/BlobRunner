using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameBaseState
{
    public GameStatus Status { get; }
    public void OnEnterState();

    public void OnExitState();
}
