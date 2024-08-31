using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private Vector3 rotateAxis = Vector3.forward;
    
    private bool isActive = false;

    private void OnEnable()
    {
        isActive = true;
    }

    private void OnDisable()
    {
        isActive = false;
    }

    void Update()
    {
        if (isActive)
        {
            transform.Rotate(rotateAxis, rotationSpeed * Time.deltaTime);
            
        }
    }
}
