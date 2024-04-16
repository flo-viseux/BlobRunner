using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputs : MonoBehaviour
{
    #region API
    public event Action OnJump;
    #endregion

    #region UnityMethods
    private void Update()
    {
        if (Input.touchCount > 0 &&  Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            OnJump?.Invoke();
            Debug.Log("OnJump");
        }
    }
    #endregion
}
