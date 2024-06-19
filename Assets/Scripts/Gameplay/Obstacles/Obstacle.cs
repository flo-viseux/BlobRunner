using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    #region UnityMethods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.name == "Player")
        {
//            Debug.Log("Player Touched");
            
        }
    }
    #endregion
}
