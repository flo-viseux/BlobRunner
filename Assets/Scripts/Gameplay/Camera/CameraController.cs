using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private Camera camera;
    #endregion

    #region UnityMethods
    private void LateUpdate()
    {
        camera.rect = ScreenController.GetSafeRect();
    }
    #endregion

}
