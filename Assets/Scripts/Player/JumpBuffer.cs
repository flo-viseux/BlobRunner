using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBuffer : MonoBehaviour
{
    public event Action OnJumpBuffer;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            OnJumpBuffer?.Invoke();
        }
    }
}
