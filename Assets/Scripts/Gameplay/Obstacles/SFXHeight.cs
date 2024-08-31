using System;
using System.Collections;
using System.Collections.Generic;
using Runner.Player;
using UnityEngine;

public class SFXHeight : MonoBehaviour
{
    private Transform playerTransform;
    private float floorHeight = 3.8f;
    private float groundHeight = -5.75f;
    
    [SerializeField] private AudioSource _source;
    private bool isActive = false;

    private void Start()
    {
        playerTransform = FindObjectOfType<Controller>().GetComponent<Transform>();
    }

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
            if (playerTransform == null)
            {
                Debug.Log("Player not found");
                return;
            }
            float playerHeight = playerTransform.position.y;
            float objectHeight = transform.position.y;

            if (playerHeight >= groundHeight && playerHeight <= floorHeight && objectHeight >= groundHeight && objectHeight <= floorHeight)
            {
                _source.volume = 1f;
            }
            else if (objectHeight > floorHeight)
            {
                _source.volume = 0.1f;
            }
            
        }
    }
}
