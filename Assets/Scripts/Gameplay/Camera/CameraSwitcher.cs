using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private Animator animator;

    [SerializeField] private Button button;
    #endregion

    #region Attributes
    private bool isDefaultCamera = true;
    #endregion

    #region API
    public static CameraSwitcher Instance = null;

    public void SwitchCamera()
    {
        //Debug.Log("SwitchCamera");

        isDefaultCamera = !isDefaultCamera;

        if (isDefaultCamera)
            animator.Play("Default");
        else
            animator.Play("Large");
    }
    #endregion

    #region UnityMethods
    private void Awake()
    {
        if(Instance == null) 
            Instance = this;
        else
            Destroy(this);
        
        isDefaultCamera = true;
        button.onClick.AddListener(SwitchCamera);
    }
    #endregion
}
