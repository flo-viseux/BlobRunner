using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private Transform player = null;

    [SerializeField] private float yMin = 0f;
    #endregion

    #region UnityMethods
    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x,
                                        Mathf.Max(player.position.y + 5.2f, yMin),
                                        transform.position.z);
    }
    #endregion
}
