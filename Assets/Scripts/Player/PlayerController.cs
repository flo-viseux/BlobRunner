using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputManager inputManager;
        private PlayerStateMachine stateMachine;

        private void Start()
        {
            stateMachine = new PlayerStateMachine(this, inputManager);
        }
    }
}
