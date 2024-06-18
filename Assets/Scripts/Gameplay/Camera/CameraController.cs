using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//[ExecuteAlways]
public class CameraController : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private Camera camera;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        Camera overlayCamera = GameObject.Find("CameraUIOverlay").GetComponent<Camera>();

        camera.GetUniversalAdditionalCameraData().cameraStack.Add(overlayCamera);
    }

    private void LateUpdate()
    {
        camera.rect = ScreenController.GetSafeRect();
    }
    #endregion

}
