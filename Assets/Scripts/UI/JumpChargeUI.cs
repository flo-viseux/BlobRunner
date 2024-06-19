using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpChargeUI : MonoBehaviour
{

    [SerializeField] private Slider jumpChargeSlider;
    [SerializeField] private float decreaseDuration = 0.2f;
    
    public void InitChargeSlider(float maxValue)
    {
        jumpChargeSlider.value = 0f;
        jumpChargeSlider.maxValue = maxValue;
    }

    public void UpdateCharge(float value)
    {
        jumpChargeSlider.value = value;
    }

    public void ResetSlider()
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
