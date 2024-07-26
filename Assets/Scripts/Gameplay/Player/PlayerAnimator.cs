using System;
using System.Collections;
using System.Collections.Generic;
using Runner.Player;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Controller _player;
    [SerializeField] private float landAnimDuration = 0.1f;
    [SerializeField] private float jumpAnimDuration = 0.1f;
    
    private Animator _animator;
    private const int baseLayer = 0;
    private const int invulnerableLayer = 1;

    private string _currentState;
    // Base Layer Animations
    private static readonly string Run = "Player - Run";
    private static readonly string Jump = "Player - Jump";
    private static readonly string InAir = "Player - InAir";
    private static readonly string Land = "Player - Landing";
    // Invulnerable Layer Animations
    private static readonly string OriginalColor = "Player - OriginalColor";
    private static readonly string SwitchColor = "Player - InvulnerableColorSwitch";

    private bool _run;
    private bool _grounded;
    private float _lockAnim;
    private bool _jump;
    private bool _landed;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _currentState = InAir;
    }

    // private void OnEnable()
    // {
    //     _player.Jumped += OnJump;
    //     _player.Run += OnRun;
    //     _player.GroundChange += OnGroundChange;
    // }
    //
    // private void OnDisable()
    // {
    //     _player.Jumped -= OnJump;
    //     _player.Run -= OnRun;
    //     _player.GroundChange -= OnGroundChange;
    // }

    private void OnJump()
    {
        _jump = true;
    }
        
    private void OnRun()
    {
        _run = true;
    } 
    
    private void OnGroundChange(bool grounded, float velocity)
    {
        _grounded = grounded;
        _landed = velocity == 0f;
    }

    private void Update()
    {
        string state = GetState();
        
        if (state != _currentState)
        {
            _animator.CrossFade(state,0,baseLayer);
            _currentState = state;
            
            // reset triggers
            _run = false;
            _jump = false;
            _landed = false;
        }
    }

    private string GetState()
    {
        if (Time.time < _lockAnim)
        {
            return _currentState;
        }
        
        if (_jump) return LockState(Jump, jumpAnimDuration);
        if (_landed) return LockState(Land, landAnimDuration);
        if (!_grounded) return InAir;
        return Run;
    }

    private string LockState(string state, float time)
    {
        _lockAnim = Time.time + time;
        return state;
    }

    public void LaunchInvulnerableAnim(bool isLaunched)
    {
        if (isLaunched)
        {
            _animator.CrossFade(SwitchColor,0,invulnerableLayer);
        }
        else
        {
            _animator.CrossFade(OriginalColor,0,invulnerableLayer);
        }
    }

    private string DebugState(string state)
    {
        if (state == Run) return "Player - Run";
        if (state == Jump) return "Player - Jump";
        if (state == InAir) return "Player - InAir";
        if (state == Land) return "Player - Land";
        return "No State";
    }
}
