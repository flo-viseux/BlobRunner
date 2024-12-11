using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    #region Enum
    public enum CameraState
    {
        Default,
        Medium,
        Large
    }
    #endregion

    #region SerializedFields
    [SerializeField] private Animator animator;
    #endregion

    #region Attributes
    private CameraState currentState;
    #endregion

    #region API
    public static CameraSwitcher Instance = null;

    public void SwitchCamera(CameraState newState)
    {
        //Debug.Log("SwitchCamera");
        currentState = newState;

        switch (currentState)
        {
            case CameraState.Default:
                animator.Play("Default");
                break;
            case CameraState.Medium:
                animator.Play("Medium");
                break;
            case CameraState.Large:
                animator.Play("Large");
                break;
        }
    }
    #endregion

    #region UnityMethods
    private void Awake()
    {
        if(Instance == null) 
            Instance = this;
        else
            Destroy(this);

        currentState = CameraState.Default;
    }
    #endregion
}
