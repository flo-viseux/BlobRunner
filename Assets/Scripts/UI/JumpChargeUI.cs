
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JumpChargeUI : MonoBehaviour
{
    [SerializeField] private SliderEventSO sliderEvent;
    [SerializeField] private Slider jumpChargeSlider;
    [SerializeField] private float decreaseDuration = 0.2f;

    private void OnEnable()
    {
        sliderEvent.OnInitialization += InitChargeSlider;
        sliderEvent.OnUpdate += UpdateCharge;
        sliderEvent.OnReset += ResetSlider;
    }

    private void OnDisable()
    {
        sliderEvent.OnInitialization -= InitChargeSlider;
        sliderEvent.OnUpdate -= UpdateCharge;
        sliderEvent.OnReset -= ResetSlider;
    }

    private void InitChargeSlider(float maxValue)
    {
        jumpChargeSlider.value = 0f;
        jumpChargeSlider.maxValue = maxValue;
    }

    private void UpdateCharge(float value)
    {
        jumpChargeSlider.value = value;
    }

    private void ResetSlider()
    {
        StartCoroutine(DecreaseCharge());
    }
    
    private IEnumerator DecreaseCharge()
    {
        float startValue = jumpChargeSlider.value;
        float time = 0;

        while (time < decreaseDuration)
        {
            jumpChargeSlider.value = Mathf.Lerp(startValue, 0, time / decreaseDuration);
            time += Time.deltaTime;
            yield return null;
        }

        jumpChargeSlider.value = 0;
    }
}
