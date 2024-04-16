using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] PlayerInputs playerInputs = null;

    [SerializeField] PlayerMovement playerMovement = null;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        playerInputs.OnJump += playerMovement.Jump;
    }
    #endregion
}
