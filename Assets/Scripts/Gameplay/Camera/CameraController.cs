using UnityEngine;

[ExecuteAlways]
public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera camera;

    private void Update()
    {
        camera.rect = ScreenController.GetSafeRect();
    }
}
