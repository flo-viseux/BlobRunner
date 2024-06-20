using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    #region UnityMethods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.name == "Player" && !PlayerInvulnerability.Instance.PlayerIsInvulnerabled)
        {
            GameManager.Instance.playerDatas.DecreaseHealth();
            PlayerInvulnerability.Instance.SetInvulerability();
            //Debug.Log("Player Touched", gameObject);
        }
    }
    #endregion
}
