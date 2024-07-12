using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private SimpleEventSO HitObstacle;
    
    #region UnityMethods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            HitObstacle.RaiseEvent();
        }
    }
    #endregion
}
