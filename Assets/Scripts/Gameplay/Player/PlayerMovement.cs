using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private Rigidbody2D rb = null;

    [SerializeField] private float jumpForce = 10;
    #endregion

    #region API
    public void Jump()
    {
        Debug.Log("Jump");
        rb.AddForce (new Vector3 (0, jumpForce, 0));
    }
    #endregion
}
