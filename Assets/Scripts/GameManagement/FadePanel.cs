using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;


public class FadePanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup.alpha = 0f;
    }

    private IEnumerator FadeOutRoutine(float fadeDuration, float stayBlackDuration)
    {
        canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(stayBlackDuration);
        canvasGroup.DOFade(0f, fadeDuration).SetEase(Ease.InOutQuad);
        canvasGroup.blocksRaycasts = false;
    }
    
    private IEnumerator FadeInRoutine(float fadeDuration, float stayBlackDuration)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = true;
        yield return new WaitForSeconds(stayBlackDuration);
        canvasGroup.DOFade(1f, fadeDuration).SetEase(Ease.InOutQuad);
    }

    public void FadeOut(float fadeDuration, float stayBlackDuration)
    {
        StartCoroutine(FadeOutRoutine(fadeDuration,stayBlackDuration));
    }

    public void FadeIn(float fadeDuration, float stayBlackDuration)
    {
        StartCoroutine(FadeInRoutine(fadeDuration, stayBlackDuration));
    }
}
