using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private CinemachineVirtualCamera camera = null;

    [SerializeField] private float shakeDuration = .5f;

    [SerializeField] private float amplitude = 1f;
    #endregion

    #region Attributes
    private CinemachineBasicMultiChannelPerlin perlin = null;
    #endregion

    #region API
    public static CameraShaker Instance;

    public void Shake()
    {
        StartCoroutine(OnShake());
    }
    #endregion

    #region UnityMethods
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        perlin = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    #endregion

    #region Private
    private IEnumerator OnShake()
    {
        perlin.m_AmplitudeGain = amplitude;
        
        yield return new WaitForSeconds(shakeDuration);

        perlin.m_AmplitudeGain = 0.0f;
    }
    #endregion
}
